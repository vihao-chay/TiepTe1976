using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonClickBG : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [Header("BG References")]
    public Image bgPressed;

    [Header("Âm thanh UI")]
    public AudioClip hoverSound;
    public AudioClip clickSound;
    private AudioSource audioSource;

    // Chỉnh lại tỷ lệ to nhỏ ở đây
    private Vector3 normalScale = Vector3.one;              // Kích thước bình thường
    private Vector3 hoverScale = Vector3.one * 1.05f;       // Rê chuột vào thì to lên 5%
    private Vector3 pressedScale = Vector3.one * 1.15f;     // Bấm vào thì to lên 15% (hoặc bạn có thể đổi thành 0.9f để làm hiệu ứng lún xuống)

    private bool isHovered = false;

    void Start()
    {
        // LUÔN BẬT BACKGROUND NGAY TỪ ĐẦU
        if (bgPressed != null)
        {
            bgPressed.enabled = true;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;

        // Phình to ra một chút khi rê chuột
        transform.localScale = hoverScale;

        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;

        // Trả về kích thước bình thường khi chuột rời đi
        transform.localScale = normalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Khi ấn chuột xuống thì đổi kích thước thành pressedScale
        transform.localScale = pressedScale;

        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Khi nhả chuột ra, kiểm tra xem chuột còn nằm trên nút không
        if (isHovered)
        {
            transform.localScale = hoverScale;
        }
        else
        {
            transform.localScale = normalScale;
        }
    }
}