using UnityEngine;

public class TrapSound : MonoBehaviour
{
    [Header("Âm thanh khi bẫy kích hoạt (Ví dụ: Tiếng nổ, sập chông)")]
    public AudioClip trapClip;
    [Range(0f, 1f)] public float volume = 1f;

    private bool isActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;

        // MỚI: Chỉ cần có Rigidbody (có khối lượng vật lý như xe tải, bánh xe, nhân vật) đạp trúng là nổ!
        if (other.GetComponentInParent<CarMovement>() != null || other.CompareTag("Player") || other.attachedRigidbody != null)
        {
            isActivated = true;

            if (trapClip != null)
            {
                // Tự động đẻ ra một cái "Loa 2D Ảo" để phát tiếng nổ cực to
                GameObject loaNo = new GameObject("Am_Thanh_No_Min");
                AudioSource audio = loaNo.AddComponent<AudioSource>();

                audio.clip = trapClip;
                audio.volume = volume;

                // QUAN TRỌNG NHẤT: Ép thành âm thanh 2D (Camera ở xa mấy cũng nghe nổ sập nhà)
                audio.spatialBlend = 0f;

                audio.Play();

                // Lệnh tự động dọn rác: Xóa cái loa ảo đi sau khi tiếng nổ phát xong (để game không bị lag)
                Destroy(loaNo, trapClip.length);
            }
        }
    }
}