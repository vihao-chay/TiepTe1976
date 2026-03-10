using UnityEngine;
using TMPro;

public class MissionTimer : MonoBehaviour
{
    [Header("Giao diện Đồng Hồ")]
    public TextMeshProUGUI timerText;

    [Header("Thời gian nhiệm vụ (Tính bằng giây)")]
    public float totalMissionTime = 120f; // Mặc định 120 giây (2 phút)

    private float timeRemaining;
    private bool isTimerRunning = false;

    void Start()
    {
        // Ghi nhớ thời gian và giấu đồng hồ đi lúc mới vào game (chưa nhận nhiệm vụ)
        timeRemaining = totalMissionTime;
        UpdateTimerUI();

        if (timerText != null) timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Trừ dần thời gian thực
                UpdateTimerUI();
            }
            else
            {
                // KHI THỜI GIAN VỀ 0
                timeRemaining = 0;
                isTimerRunning = false;
                UpdateTimerUI();
                HetGio();
            }
        }
    }

    // --- HÀM MỞ KHÓA: NÚT "ACCEPT" SẼ GỌI VÀO ĐÂY ---
    public void StartMissionTimer()
    {
        isTimerRunning = true;
        if (timerText != null) timerText.gameObject.SetActive(true); // Bật hiện đồng hồ lên
    }

    // --- HÀM TẠM DỪNG: NẾU VỀ ĐÍCH THÀNH CÔNG THÌ GỌI VÀO ĐÂY ---
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Tự động quy đổi số giây ra định dạng Phút:Giây (Ví dụ 02:30)
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void HetGio()
    {
        Debug.Log("HẾT GIỜ! Nhiệm vụ thất bại.");
        if (timerText != null)
        {
            timerText.text = "HẾT GIỜ!";
            timerText.color = Color.red; // Đổi sang màu đỏ cảnh báo
        }

        // Đoạn này sau này chúng ta sẽ viết code Màn hình Đen - Thất Bại
    }
}