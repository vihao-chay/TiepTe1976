using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    public float speed = 5f;    // Tốc độ nhấp nhô
    public float height = 0.2f; // Độ cao nhấp nhô (nhích lên xuống bao nhiêu)

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // Ghi nhớ vị trí ban đầu
    }

    void Update()
    {
        // Tính toán vị trí mới dựa trên sóng hình Sin để tạo cảm giác lơ lửng mượt mà
        float newY = startPos.y + (Mathf.Sin(Time.time * speed) * height);
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}