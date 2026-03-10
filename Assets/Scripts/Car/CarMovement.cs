using UnityEngine;
using TMPro;

public class CarMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float boostSpeed = 13f;
    public float turnSpeed = 50f;

    [Header("Camera Cố Định")]
    public Transform carCamera;

    [Header("Tính năng Checkpoint (MỚI)")]
    public float resetCooldown = 5f;   // Nút R mất 5 giây để làm lạnh
    private float nextResetTime = 0f;

    // Tọa độ và Góc quay của Checkpoint hiện tại
    private Vector3 checkpointPosition;
    private Quaternion checkpointRotation;

    [Header("Âm thanh & Âm lượng")]
    public AudioClip startEngineClip;
    [Range(0f, 1f)] public float engineVolume = 1f;

    public AudioClip rollingClip;
    [Range(0f, 1f)] public float rollingVolume = 1f;

    public AudioClip crashClip;
    [Range(0f, 1f)] public float crashVolume = 1f;

    public AudioClip doorCloseClip;
    [Range(0f, 1f)] public float doorCloseVolume = 1f;

    public AudioClip musicClip;
    [Range(0f, 1f)] public float musicVolume = 0.4f;

    [Header("Giao diện UI")]
    public GameObject radioUI_Parent;
    public TextMeshProUGUI radioText;

    public GameObject rewindUI_Parent;
    public TextMeshProUGUI rewindText;

    public Vector3 centerOfMassOffset = new Vector3(0f, -1f, 0f);
    private Rigidbody rb;

    private AudioSource startEngineSource;
    private AudioSource rollingSource;
    private AudioSource musicSource;

    private bool hasStartedMoving = false;
    private bool isMusicPlaying = false;
    private bool wasPlayerInCar = false;

    // --- KHÓA ĐIỀU KHIỂN KHI BỊ TRAP ---
    private bool canControl = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMassOffset;

        startEngineSource = gameObject.AddComponent<AudioSource>();
        startEngineSource.playOnAwake = false;
        startEngineSource.loop = false;

        rollingSource = gameObject.AddComponent<AudioSource>();
        rollingSource.playOnAwake = false;
        rollingSource.loop = true;
        rollingSource.clip = rollingClip;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.clip = musicClip;
        musicSource.spatialBlend = 0f;

        if (radioUI_Parent != null) radioUI_Parent.SetActive(false);
        if (rewindUI_Parent != null) rewindUI_Parent.SetActive(false);
    }

    private void KichHoatLenXe()
    {
        if (wasPlayerInCar) return;
        wasPlayerInCar = true;
        hasStartedMoving = false;

        if (startEngineSource != null && startEngineClip != null && engineVolume > 0)
        {
            startEngineSource.Stop();
            startEngineSource.PlayOneShot(startEngineClip);
        }

        if (radioUI_Parent != null) radioUI_Parent.SetActive(true);
        if (rewindUI_Parent != null) rewindUI_Parent.SetActive(true);

        // MỚI: LƯU CHECKPOINT NGAY LÚC BƯỚC LÊN XE
        checkpointPosition = transform.position;
        checkpointRotation = transform.rotation;

        UpdateRadioText();
    }

    private void KichHoatXuongXe()
    {
        if (!wasPlayerInCar) return;
        wasPlayerInCar = false;

        if (startEngineSource != null) startEngineSource.Stop();
        if (rollingSource != null) rollingSource.Stop();
        if (musicSource != null) musicSource.Stop();

        isMusicPlaying = false;
        if (radioUI_Parent != null) radioUI_Parent.SetActive(false);
        if (rewindUI_Parent != null) rewindUI_Parent.SetActive(false);

        if (doorCloseClip != null && doorCloseVolume > 0f)
        {
            AudioSource.PlayClipAtPoint(doorCloseClip, transform.position, doorCloseVolume);
        }
    }

    void OnEnable()
    {
        if (carCamera != null && carCamera.gameObject.activeInHierarchy) KichHoatLenXe();
    }

    void OnDisable()
    {
        KichHoatXuongXe();
    }

    void Update()
    {
        if (startEngineSource != null) startEngineSource.volume = engineVolume;
        if (rollingSource != null) rollingSource.volume = rollingVolume;
        if (musicSource != null) musicSource.volume = musicVolume;

        bool isPlayerInCar = carCamera != null && carCamera.gameObject.activeInHierarchy;

        if (isPlayerInCar) KichHoatLenXe();
        else KichHoatXuongXe();

        if (isPlayerInCar)
        {
            // BẬT / TẮT RADIO (Phím H)
            if (Input.GetKeyDown(KeyCode.H))
            {
                isMusicPlaying = !isMusicPlaying;
                if (isMusicPlaying)
                {
                    if (musicSource != null && musicClip != null) musicSource.Play();
                }
                else
                {
                    if (musicSource != null) musicSource.Pause();
                }
                UpdateRadioText();
            }

            // MỚI: QUAY VỀ CHECKPOINT (Phím R)
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (Time.time >= nextResetTime)
                {
                    // Đưa xe về tọa độ Checkpoint, nhấc bổng lên 2 mét để rơi xuống an toàn
                    rb.position = checkpointPosition + new Vector3(0, 2f, 0);
                    rb.rotation = Quaternion.Euler(0f, checkpointRotation.eulerAngles.y, 0f);

                    // Phanh khẩn cấp xóa quán tính rơi
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;

                    // Khởi động thời gian làm lạnh
                    nextResetTime = Time.time + resetCooldown;
                }
            }
        }
    }

    void UpdateRadioText()
    {
        if (radioText != null)
        {
            if (isMusicPlaying) radioText.text = "Nhấn [H]: Tắt Radio";
            else radioText.text = "Nhấn [H]: Bật Radio";
        }
    }

    void FixedUpdate()
    {
        if (carCamera == null || !carCamera.gameObject.activeInHierarchy) return;

        // CẬP NHẬT CHỮ CHECKPOINT
        if (rewindText != null)
        {
            if (Time.time < nextResetTime)
            {
                int timeLeft = Mathf.CeilToInt(nextResetTime - Time.time);
                rewindText.text = "Nhấn [R]: Đang hồi (" + timeLeft + "s)";
            }
            else
            {
                rewindText.text = "Nhấn [R]: Quay lại";
            }
        }

        if (!canControl) return;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        bool isBoosting = Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1);
        float currentSpeed = isBoosting ? boostSpeed : moveSpeed;

        Vector3 moveDirection = transform.forward * vertical * currentSpeed;
        moveDirection.y = rb.linearVelocity.y;

        rb.linearVelocity = moveDirection;

        if (vertical != 0)
        {
            float turnMultiplier = vertical > 0 ? 1f : -1f;
            Quaternion turnRotation = Quaternion.Euler(0f, horizontal * turnSpeed * turnMultiplier * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        float speedMagnitude = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        bool isMoving = speedMagnitude > 0.5f;

        if (isMoving)
        {
            if (!hasStartedMoving)
            {
                if (startEngineSource != null) startEngineSource.Stop();
                hasStartedMoving = true;
            }

            if (rollingSource != null && !rollingSource.isPlaying && rollingClip != null)
            {
                rollingSource.Play();
            }
        }
        else
        {
            if (rollingSource != null && rollingSource.isPlaying)
            {
                rollingSource.Stop();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "main_character" || collision.gameObject.CompareTag("Player")) return;

        if (collision.relativeVelocity.magnitude > 2.5f)
        {
            if (startEngineSource != null && crashClip != null && crashVolume > 0)
            {
                startEngineSource.PlayOneShot(crashClip, crashVolume);
            }
        }
    }

    public void DisableControl(float time)
    {
        canControl = false;
        Invoke(nameof(EnableControl), time);
    }

    void EnableControl()
    {
        canControl = true;
    }

    // --- HÀM MỞ CHO CÁC TRẠM CHECKPOINT TRÊN ĐƯỜNG GỌI VÀO ---
    public void LuuCheckpointMoi(Vector3 toaDoMoi, Quaternion gocQuayMoi)
    {
        checkpointPosition = toaDoMoi;
        checkpointRotation = gocQuayMoi;
        Debug.Log("Đã lưu Checkpoint mới trên đường đua!");
    }
}