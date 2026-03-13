using UnityEngine;
using TMPro;

public class MissionTimer : MonoBehaviour
{
    [Header("Giao diện Đồng Hồ")]
    public GameObject timerBackground; // Kéo TimerBackground vào đây
    public TextMeshProUGUI timerText;

    [Header("Thời gian nhiệm vụ (Tính bằng giây)")]
    public float totalMissionTime = 120f; // Mặc định 120 giây (2 phút)

    private float timeRemaining;
    private bool isTimerRunning = false;

    void Start()
    {
        timeRemaining = totalMissionTime;
        UpdateTimerUI();

        // Giấu cả cụm nền đen và chữ đi lúc mới vào game
        if (timerBackground != null) timerBackground.SetActive(false);
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                UpdateTimerUI();
                HetGio();
            }
        }
    }

    // --- HÀM ĐƯỢC GỌI KHI BẤM NÚT ACCEPT CỦA NPC ---
    public void StartMissionTimer()
    {
        isTimerRunning = true;
        // Bật hiển thị cả cụm đồng hồ lên
        if (timerBackground != null) timerBackground.SetActive(true);
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void HetGio()
    {
        Debug.Log("HẾT GIỜ! Nhiệm vụ thất bại.");
        GameOutro outro = FindFirstObjectByType<GameOutro>();
        if (outro != null)
        {
            outro.StartFinalExit("NHIỆM VỤ THẤT BẠI!\nBẠN ĐÃ HẾT THỜI GIAN TIẾP TẾ.");
        }
    }
}