using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Tự động thêm AudioSource vào nút nếu chưa có
[RequireComponent(typeof(AudioSource))]
public class ButtonClickBG : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [Header("BG References")]
    public Image bgPressed;

    [Header("Âm thanh UI")]
    public AudioClip hoverSound; // Kéo file âm thanh lúc rê chuột vào đây
    public AudioClip clickSound; // Kéo file âm thanh lúc bấm chuột vào đây
    private AudioSource audioSource;

    private Vector3 hoverScale = Vector3.one;
    private Vector3 pressedScale = Vector3.one * 1.2f;
    private bool isHovered = false;
    private bool isPressed = false;

    void Start()
    {
        if (bgPressed != null)
        {
            bgPressed.enabled = false;
        }

        // Lấy cái loa (AudioSource) trên nút để chuẩn bị phát nhạc
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; // Tắt tự động phát khi mới mở game
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        if (bgPressed != null && !isPressed)
        {
            bgPressed.enabled = true;
            transform.localScale = hoverScale;
        }

        // PHÁT ÂM THANH HOVER
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        if (bgPressed != null)
        {
            bgPressed.enabled = false;
            transform.localScale = hoverScale;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        if (bgPressed != null && isHovered)
        {
            bgPressed.enabled = true;
            transform.localScale = pressedScale;
        }

        // PHÁT ÂM THANH CLICK
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        if (bgPressed != null && isHovered)
        {
            transform.localScale = hoverScale;
        }
    }
}