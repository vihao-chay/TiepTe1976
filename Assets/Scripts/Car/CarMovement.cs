using UnityEngine;
using TMPro;

public class CarMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float boostSpeed = 13f;
    public float turnSpeed = 50f;

    [Header("Camera Cố Định")]
    public Transform carCamera;

    [Header("Âm thanh & Âm lượng (Kéo thanh trượt để chỉnh)")]
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

    [Header("Giao diện Radio")]
    public GameObject radioUI;
    public TextMeshProUGUI radioText;

    public Vector3 centerOfMassOffset = new Vector3(0f, -1f, 0f);
    private Rigidbody rb;

    private AudioSource startEngineSource;
    private AudioSource rollingSource;
    private AudioSource musicSource;

    private bool hasStartedMoving = false;
    private bool isMusicPlaying = false;

    // Cờ đánh dấu để sửa triệt để lỗi "nhớ nhầm"
    private bool wasPlayerInCar = false;

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

        if (radioUI != null) radioUI.SetActive(false);
    }

    // --- HÀM BẢO VỆ 1: XỬ LÝ KHI LÊN XE ---
    private void KichHoatLenXe()
    {
        if (wasPlayerInCar) return; // Tránh chạy lặp lại
        wasPlayerInCar = true;      // Ghi nhớ là đã lên xe
        hasStartedMoving = false;

        if (startEngineSource != null && startEngineClip != null && engineVolume > 0)
        {
            startEngineSource.Stop();
            startEngineSource.PlayOneShot(startEngineClip);
        }

        if (radioUI != null) radioUI.SetActive(true);
        UpdateRadioText();
    }

    // --- HÀM BẢO VỆ 2: XỬ LÝ KHI XUỐNG XE ---
    private void KichHoatXuongXe()
    {
        if (!wasPlayerInCar) return; // Tránh chạy lặp lại
        wasPlayerInCar = false;      // Ghi nhớ là đã xuống xe (Rất quan trọng!)

        if (startEngineSource != null) startEngineSource.Stop();
        if (rollingSource != null) rollingSource.Stop();
        if (musicSource != null) musicSource.Stop();

        isMusicPlaying = false;
        if (radioUI != null) radioUI.SetActive(false);

        // Phát tiếng đóng cửa bằng một Loa ảo độc lập (kể cả code này bị tắt thì tiếng vẫn vang lên)
        if (doorCloseClip != null && doorCloseVolume > 0f)
        {
            AudioSource.PlayClipAtPoint(doorCloseClip, transform.position, doorCloseVolume);
        }
    }

    // Bắt sự kiện khi code bị bật/tắt đột ngột bởi Script khác
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

        // Tự động kiểm tra liên tục xem có ngồi trên xe không
        if (isPlayerInCar) KichHoatLenXe();
        else KichHoatXuongXe();

        if (isPlayerInCar && Input.GetKeyDown(KeyCode.H))
        {
            isMusicPlaying = !isMusicPlaying;
            if (isMusicPlaying) { if (musicSource != null && musicClip != null) musicSource.Play(); }
            else { if (musicSource != null) musicSource.Pause(); }
            UpdateRadioText();
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
}