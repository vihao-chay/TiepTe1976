using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Giao diện")]
    public GameObject pausePanel; // Kéo PausePanel vào đây
    public string menuSceneName = "MainStory"; // Tên màn hình Menu

    private bool isPaused = false;

    void Update()
    {
        // Kiểm tra nếu người chơi ấn nút ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // Nếu đang dừng thì chơi tiếp
            }
            else
            {
                PauseGame(); // Nếu đang chơi thì dừng lại
            }
        }
    }

    // Hàm gọi khi ấn nút Tiếp Tục (hoặc ấn ESC lần nữa)
    public void ResumeGame()
    {
        pausePanel.SetActive(false); // Ẩn màn hình đen
        Time.timeScale = 1f;         // TRẢ LẠI THỜI GIAN BÌNH THƯỜNG
        isPaused = false;

        // Tắt chuột và khóa lại để lái xe
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Hàm gọi khi ấn ESC lúc đang chơi
    void PauseGame()
    {
        pausePanel.SetActive(true);  // Hiện màn hình đen và nút
        Time.timeScale = 0f;         // DỪNG ĐÓNG BĂNG THỜI GIAN
        isPaused = true;

        // Hiện chuột ra để người chơi có thể click nút
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Hàm gọi khi ấn nút Thoát
    public void QuitToMenu()
    {
        // LỖI KINH ĐIỂN CẦN TRÁNH: Phải trả lại thời gian về 1 trước khi chuyển Scene. 
        // Nếu không, ra ngoài Menu game vẫn bị đóng băng!
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}