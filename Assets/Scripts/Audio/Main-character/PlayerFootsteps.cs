using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Cài đặt Âm thanh")]
    public AudioSource audioSource;
    public AudioClip walkSound; // File âm thanh đi bộ (chứa chuỗi nhiều bước)
    public AudioClip runSound;  // File âm thanh chạy (chứa chuỗi nhiều bước)

    void Start()
    {
        // Ép cái loa tự động phát lặp lại (Loop) vòng tròn khi chưa ấn Stop
        if (audioSource != null)
        {
            audioSource.loop = true;
        }
    }

    void Update()
    {
        // 1. Kiểm tra xem người chơi có đang di chuyển không
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        // 2. Kiểm tra xem có đang giữ Shift không
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1);

        if (isMoving)
        {
            // Xác định xem nên bật băng đi bộ hay băng chạy bộ
            AudioClip clipToPlay = isRunning ? runSound : walkSound;

            // Nếu loa đang tắt, hoặc đang phát nhầm băng (ví dụ đang đi bộ mà chuyển sang chạy)
            if (!audioSource.isPlaying || audioSource.clip != clipToPlay)
            {
                audioSource.clip = clipToPlay; // Đút băng vào
                audioSource.Play();            // Bật đài
            }
        }
        else
        {
            // NẾU BUÔNG TAY KHỎI BÀN PHÍM -> TẮT ĐÀI NGAY LẬP TỨC
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}