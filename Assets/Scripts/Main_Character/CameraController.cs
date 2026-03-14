using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Giao diện cản Camera (MỚI)")]
    public GameObject instructionPanel; // Kéo bảng Hướng dẫn vào đây
    public GameObject dialoguePanel;    // Kéo bảng Trò chuyện vào đây

    [Header("Cài đặt Mục tiêu")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);

    [Header("Góc nhìn Điện ảnh")]
    public float distance = 4.5f;
    public float minDistance = 0.5f;
    public float maxDistance = 10f;

    [Header("Cài đặt Chuột")]
    public float mouseSensitivity = 3.0f;
    public float yMinLimit = -15f;
    public float yMaxLimit = 60f;

    private float currentX = 0.0f;
    private float currentY = 15.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // 1. Nếu game đang tạm dừng (Bấm ESC) -> Khóa camera
        if (Time.timeScale == 0f) return;

        // 2. MỚI: Nếu bảng Hướng dẫn đang HIỆN -> Khóa camera
        if (instructionPanel != null && instructionPanel.activeSelf) return;

        // 3. MỚI: Nếu bảng Trò chuyện đang HIỆN -> Khóa camera
        if (dialoguePanel != null && dialoguePanel.activeSelf) return;

        if (target == null) return;

        // --- Các lệnh quay camera bên dưới giữ nguyên ---
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);

        distance -= Input.GetAxis("Mouse ScrollWheel") * 2f;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Vector3 pivotPosition = target.position + targetOffset;
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        transform.position = pivotPosition + rotation * new Vector3(0, 0, -distance);
        transform.LookAt(pivotPosition);
    }
}