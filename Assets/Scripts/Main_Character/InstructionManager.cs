using UnityEngine;
using UnityEngine.UI; // Cần dòng này để điều khiển Image

public class InstructionManager : MonoBehaviour
{
    [Header("Bảng Giao Diện")]
    public GameObject instructionPanel; // Kéo InstructionPanel vào đây
    public Image displayImage;          // Kéo cái Image (màn hình chiếu) vào đây

    [Header("Ảnh Hướng Dẫn (Figma)")]
    public Sprite huongDanNguoi;        // Kéo ảnh HD Người vào đây
    public Sprite huongDanXe;           // Kéo ảnh HD Xe vào đây

    void Start()
    {
        // Tắt bảng hướng dẫn khi mới vào game
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Bấm F để Bật/Tắt bảng hướng dẫn
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (instructionPanel.activeSelf)
            {
                CloseInstruction();
            }
            else
            {
                OpenInstruction();
            }
        }
    }

    // Hàm mở bảng (Mặc định hiện ảnh Người)
    public void OpenInstruction()
    {
        instructionPanel.SetActive(true);
        ShowNguoi();
    }

    // Hàm đóng bảng
    public void CloseInstruction()
    {
        instructionPanel.SetActive(false);
    }

    // Hàm được gọi khi bấm nút "Người"
    public void ShowNguoi()
    {
        displayImage.sprite = huongDanNguoi;
    }

    // Hàm được gọi khi bấm nút "Xe"
    public void ShowXe()
    {
        displayImage.sprite = huongDanXe;
    }
}