using UnityEngine;
using TMPro;

public class BookInteraction : MonoBehaviour
{
    [Header("UI References")]
    public GameObject bookPromptCanvas; // Kéo cái BookPromptCanvas vào đây
    public GameObject instructionPanel; // Bảng hướng dẫn to giữa màn hình

    private bool isPlayerNearby = false;
    private bool isPanelOpen = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            TogglePanel();
        }
    }

    void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;
        instructionPanel.SetActive(isPanelOpen);

        // Hiện chuột khi mở bảng, ẩn khi đóng
        Cursor.visible = isPanelOpen;
        Cursor.lockState = isPanelOpen ? CursorLockMode.None : CursorLockMode.Locked;

        // Khi đang mở bảng hướng dẫn thì ẩn cái chữ F bay trên đầu sách đi
        if (isPanelOpen) bookPromptCanvas.SetActive(false);
        else if (isPlayerNearby) bookPromptCanvas.SetActive(true);
    }

    public void ClosePanel()
    {
        isPanelOpen = false;
        instructionPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (isPlayerNearby) bookPromptCanvas.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra đúng xe hoặc người chơi
        if (other.CompareTag("Player") || other.GetComponentInParent<CarMovement>() != null)
        {
            isPlayerNearby = true;
            if (!isPanelOpen) bookPromptCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponentInParent<CarMovement>() != null)
        {
            isPlayerNearby = false;
            bookPromptCanvas.SetActive(false);
            if (isPanelOpen) ClosePanel();
        }
    }
}