using UnityEngine;

// Dòng này giúp tự động gắn thêm cái Loa (Audio Source) vào vật thể nếu bạn lỡ quên
[RequireComponent(typeof(AudioSource))]
public class ObjectMovementSound : MonoBehaviour
{
    [Header("Âm thanh khi chuyển động (Ví dụ: Tiếng cây đổ)")]
    public AudioClip movingClip;
    [Range(0f, 1f)] public float volume = 1f;

    // Bộ lọc chống nhiễu (Bỏ qua những rung lắc quá nhỏ)
    private float movementThreshold = 0.005f;
    private float rotationThreshold = 0.05f;

    private AudioSource audioSource;
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    void Start()
    {
        // Tự động cài đặt Loa 3D
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = movingClip;
        audioSource.loop = true; // Lặp lại tiếng kêu rắc rắc chừng nào cây còn đang đổ
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // Ép thành âm thanh 3D để đứng gần mới nghe thấy
        audioSource.minDistance = 5f;
        audioSource.maxDistance = 30f; // Vang xa 30 mét

        // Ghi nhớ vị trí ban đầu
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void Update()
    {
        audioSource.volume = volume;

        // Đo khoảng cách và góc quay so với 1 khung hình trước đó
        float moveDistance = Vector3.Distance(transform.position, lastPosition);
        float rotateAngle = Quaternion.Angle(transform.rotation, lastRotation);

        // Nếu xê dịch hoặc nghiêng ngả vượt mức cho phép -> Đang chuyển động!
        bool isMoving = (moveDistance > movementThreshold) || (rotateAngle > rotationThreshold);

        if (isMoving)
        {
            if (!audioSource.isPlaying && movingClip != null)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // Cập nhật lại vị trí để dành đo cho khung hình tiếp theo
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }
}