using UnityEngine;

public class CarInteract : MonoBehaviour
{
    [Header("Giao Diện")]
    public GameObject pressFText; // Chữ "Ấn F để lên xe"
    public GameObject pressGText; // Chữ "Ấn G để xuống xe"

    // MỚI 1: Thêm biến để chứa toàn bộ cụm nút lái xe
    public GameObject drivingUI_Group;

    [Header("Nhân Vật & Điểm Xuống")]
    public GameObject player;
    public Transform exitPoint;

    [Header("Camera & Lái Xe")]
    public GameObject playerCamera; // Main Camera của người chơi
    public GameObject carCamera;    // Camera sau đít xe
    private CarMovement carMovementScript;

    private bool playerInRange = false;
    private bool isDriving = false;

    void Start()
    {
        carMovementScript = GetComponent<CarMovement>();
        carMovementScript.enabled = false; // Mới vào game thì tắt máy xe

        if (pressFText != null) pressFText.SetActive(false);
        if (pressGText != null) pressGText.SetActive(false);

        // MỚI 2: Đảm bảo giấu cụm nút R, H, G đi lúc mới vào game
        if (drivingUI_Group != null) drivingUI_Group.SetActive(false);
    }

    void Update()
    {
        // LÊN XE (Nếu ở gần, chưa lái, và ấn F)
        if (playerInRange && !isDriving && Input.GetKeyDown(KeyCode.F))
        {
            EnterCar();
        }

        // XUỐNG XE (Nếu đang lái và ấn G)
        if (isDriving && Input.GetKeyDown(KeyCode.G))
        {
            ExitCar();
        }
    }

    void EnterCar()
    {
        isDriving = true;
        if (pressFText != null) pressFText.SetActive(false);
        if (pressGText != null) pressGText.SetActive(true);

        // MỚI 3: Bật cụm nút lái xe lên khi chui vào buồng lái
        if (drivingUI_Group != null) drivingUI_Group.SetActive(true);

        // 1. Tàng hình nhân vật chính & tắt camera người
        player.SetActive(false);
        playerCamera.SetActive(false);

        // 2. Bật camera xe & bật tính năng lái xe
        carCamera.SetActive(true);
        carMovementScript.enabled = true;
    }

    void ExitCar()
    {
        isDriving = false;
        if (pressGText != null) pressGText.SetActive(false);

        // MỚI 4: Giấu cụm nút lái xe đi khi bước xuống đất
        if (drivingUI_Group != null) drivingUI_Group.SetActive(false);

        // 1. Dịch chuyển người chơi ra cửa xe và hiện lại
        player.transform.position = exitPoint.position;
        player.SetActive(true);

        // 2. Tắt camera xe, bật lại camera người
        carCamera.SetActive(false);
        playerCamera.SetActive(true);

        // 3. Tắt tính năng lái xe
        carMovementScript.enabled = false;
    }

    // CẢM BIẾN NHẬN DIỆN NGƯỜI LẠI GẦN
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDriving)
        {
            playerInRange = true;
            if (pressFText != null) pressFText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (pressFText != null) pressFText.SetActive(false);
        }
    }
}