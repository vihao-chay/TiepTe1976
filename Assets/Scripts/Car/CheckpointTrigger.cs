using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    // Biến này để đảm bảo mỗi trạm chỉ lưu 1 lần khi đi qua, không bị spam
    private bool isActivated = false;

    void OnTriggerEnter(Collider other)
    {
        // Nếu trạm này đã được ăn rồi thì không làm gì cả
        if (isActivated) return;

        // Quét xem vật vừa đi xuyên qua có phải là chiếc xe tải không
        CarMovement xeTai = other.GetComponentInParent<CarMovement>();

        if (xeTai != null)
        {
            // Gọi chiếc xe và bắt nó ghi nhớ tọa độ + góc quay của chính cái Trạm này
            xeTai.LuuCheckpointMoi(transform.position, transform.rotation);

            isActivated = true; // Đánh dấu là trạm này đã xài

            Debug.Log("Đã lưu Checkpoint mới tại: " + gameObject.name);
        }
    }
}