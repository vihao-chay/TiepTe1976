using UnityEngine;
using UnityEngine.UI;

public class UIKeyboardFeedback : MonoBehaviour
{
    [Header("Phím cần bấm")]
    public KeyCode myKey; // Nút trên bàn phím (Vd: H, R, G)

    [Header("Hiệu ứng hiển thị")]
    public Image darkBackground; // Kéo cái lớp nền tối (DarkBG) vào đây

    private Color normalColor;
    private Color pressedColor = new Color(1f, 1f, 1f, 0.5f); // Chuyển thành màu trắng mờ khi bị ấn

    private Vector3 normalScale = Vector3.one;
    private Vector3 pressedScale = Vector3.one * 0.9f; // Hơi lún xuống một tí

    void Start()
    {
        if (darkBackground != null)
        {
            normalColor = darkBackground.color; // Ghi nhớ màu gốc ban đầu
        }
    }

    void Update()
    {
        // Khi bắt đầu ấn phím xuống
        if (Input.GetKeyDown(myKey))
        {
            if (darkBackground != null) darkBackground.color = pressedColor;
            transform.localScale = pressedScale;
        }
        // Khi nhả phím ra
        else if (Input.GetKeyUp(myKey))
        {
            if (darkBackground != null) darkBackground.color = normalColor;
            transform.localScale = normalScale;
        }
    }
}