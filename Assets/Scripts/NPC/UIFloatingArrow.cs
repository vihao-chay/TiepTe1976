using UnityEngine;

public class UIFloatingArrow : MonoBehaviour
{
    [Header("Cài đặt chuyển động")]
    public float speed = 3f; // Tốc độ nhấp nháy
    public float amount = 5f; // Độ cao di chuyển lên xuống

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition; // Lưu lại vị trí ban đầu
    }

    void Update()
    {
        // Phép toán Sin giúp vật thể trôi lên trôi xuống cực kỳ mượt mà
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amount;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}