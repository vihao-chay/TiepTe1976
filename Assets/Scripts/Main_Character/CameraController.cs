using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Cài đặt Mục tiêu")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0f, 1.5f, 0f); // Điểm Camera luôn nhìn vào (Ngang vai/cổ)

    [Header("Góc nhìn Điện ảnh (Wuthering Waves)")]
    public float distance = 4.5f; // Khoảng cách xa ra sau lưng
    public float minDistance = 0.5f; // Zoom in gần nhất
    public float maxDistance = 10f; // Zoom out xa nhất

    [Header("Cài đặt Chuột")]
    public float mouseSensitivity = 3.0f;
    public float yMinLimit = -15f; // Góc ngước lên trời tối đa
    public float yMaxLimit = 60f; // Góc cúi xuống đất tối đa

    private float currentX = 0.0f;
    private float currentY = 15.0f; // MỚI: Mặc định lúc vào game camera sẽ hơi cúi nhẹ xuống

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // 🚨 THÊM DÒNG NÀY: Ngăn camera quay khi game đang tạm dừng (Time.timeScale == 0)
        if (Time.timeScale == 0f) return;

        if (target == null) return;

        // 1. Lấy tín hiệu xoay chuột
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);

        // 2. Tính năng Zoom bằng con lăn chuột (Cuộn lên/xuống)
        distance -= Input.GetAxis("Mouse ScrollWheel") * 2f;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // 3. Tính toán vị trí tâm xoay (Cổ nhân vật)
        Vector3 pivotPosition = target.position + targetOffset;
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // 4. Di chuyển camera lùi ra sau và ép nhìn thẳng vào tâm
        transform.position = pivotPosition + rotation * new Vector3(0, 0, -distance);
        transform.LookAt(pivotPosition);
    }
}