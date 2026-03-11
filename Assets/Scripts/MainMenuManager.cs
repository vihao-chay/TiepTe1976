using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // MỚI: Hàm Start sẽ chạy ngay khi Menu vừa hiện lên
    void Start()
    {
        // Hiện lại con trỏ chuột (đề phòng trường hợp trong game bị ẩn)
        Cursor.visible = true;

        // Giải phóng chuột để có thể di chuyển tự do (không bị khóa vào tâm màn hình)
        Cursor.lockState = CursorLockMode.None;
    }

    // Hàm này gắn vào nút Play
    public void BamNutPlay()
    {
        SceneManager.LoadScene("mapgame");
    }

    // Hàm này gắn vào nút Quit
    public void BamNutQuit()
    {
        Debug.Log("Đã thoát game!");

        // 1. Lệnh này để tắt game khi đã xuất ra file .exe (Game thật)
        Application.Quit();

        // 2. Lệnh này ép Unity Editor tự động dừng chế độ Play (Dành cho lúc Test)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}