using UnityEngine;

public enum QuestState
{
    NotStarted,
    InProgress,
    Completed,
    Reported
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Tiến trình nhiệm vụ")]
    public int currentDay = 1;
    public QuestState state = QuestState.NotStarted;

    [Header("NPC")]
    public NPCInteract npc1;
    public NPCInteract npc2;
    public NPCInteract npc3;
    public NPCInteract npc4;

    [Header("Chỉ đường")]
    public VehicleQuestArrow vehicleArrow;

    [Header("Hàng hóa đợt 1")]
    public GameObject cargoOnTruck;
    public GameObject cargoAtDestination;

    [Header("Hàng hóa đợt 2")]
    public GameObject cargoOnTruckDay2;
    public GameObject cargoAtDestinationDay2;

    [Header("Hàng hóa đợt 3")]
    public GameObject cargoOnTruckDay3;
    public GameObject cargoAtDestinationDay3;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateQuestUI();

        if (vehicleArrow != null)
            vehicleArrow.ClearTarget();

        // Tắt toàn bộ hàng hóa
        if (cargoOnTruck != null)
            cargoOnTruck.SetActive(false);

        if (cargoAtDestination != null)
            cargoAtDestination.SetActive(false);

        if (cargoOnTruckDay2 != null)
            cargoOnTruckDay2.SetActive(false);

        if (cargoAtDestinationDay2 != null)
            cargoAtDestinationDay2.SetActive(false);

        if (cargoOnTruckDay3 != null)
            cargoOnTruckDay3.SetActive(false);

        if (cargoAtDestinationDay3 != null)
            cargoAtDestinationDay3.SetActive(false);

        // Tắt mũi tên NPC giao hàng
        if (npc2 != null && npc2.questArrow != null)
            npc2.questArrow.SetActive(false);

        if (npc3 != null && npc3.questArrow != null)
            npc3.questArrow.SetActive(false);

        if (npc4 != null && npc4.questArrow != null)
            npc4.questArrow.SetActive(false);
    }

    public NPCInteract GetCurrentTargetNPC()
    {
        if (currentDay == 1) return npc2;
        if (currentDay == 2) return npc3;
        if (currentDay == 3) return npc4;
        return null;
    }

    public GameObject GetCurrentCargoOnTruck()
    {
        if (currentDay == 1) return cargoOnTruck;
        if (currentDay == 2) return cargoOnTruckDay2;
        if (currentDay == 3) return cargoOnTruckDay3;
        return null;
    }

    public GameObject GetCurrentCargoAtDestination()
    {
        if (currentDay == 1) return cargoAtDestination;
        if (currentDay == 2) return cargoAtDestinationDay2;
        if (currentDay == 3) return cargoAtDestinationDay3;
        return null;
    }

    public void StartSecondTrip()
    {
        currentDay = 2;
        state = QuestState.NotStarted;

        if (vehicleArrow != null)
            vehicleArrow.ClearTarget();

        if (npc2 != null)
        {
            npc2.isFinished = true;
            if (npc2.questArrow != null)
                npc2.questArrow.SetActive(false);
        }

        if (npc3 != null)
        {
            npc3.gameObject.SetActive(true);
            npc3.isFinished = false;
            if (npc3.questArrow != null)
                npc3.questArrow.SetActive(false);
        }

        if (cargoOnTruck != null)
            cargoOnTruck.SetActive(false);

        if (cargoAtDestination != null)
            cargoAtDestination.SetActive(false);

        if (cargoOnTruckDay2 != null)
            cargoOnTruckDay2.SetActive(false);

        if (cargoAtDestinationDay2 != null)
            cargoAtDestinationDay2.SetActive(false);

        if (cargoOnTruckDay3 != null)
            cargoOnTruckDay3.SetActive(false);

        if (cargoAtDestinationDay3 != null)
            cargoAtDestinationDay3.SetActive(false);

        if (npc1 != null)
        {
            npc1.isFinished = false;
            if (npc1.questArrow != null)
                npc1.questArrow.SetActive(true);
        }

        UpdateQuestUI();
        Debug.Log("Đã bắt đầu chuyến hàng đợt 2");
    }

    public void StartThirdTrip()
    {
        currentDay = 3;
        state = QuestState.NotStarted;

        if (vehicleArrow != null)
            vehicleArrow.ClearTarget();

        if (npc3 != null)
        {
            npc3.isFinished = true;
            if (npc3.questArrow != null)
                npc3.questArrow.SetActive(false);
        }

        if (npc4 != null)
        {
            npc4.gameObject.SetActive(true);
            npc4.isFinished = false;
            if (npc4.questArrow != null)
                npc4.questArrow.SetActive(false);
        }

        if (cargoOnTruck != null)
            cargoOnTruck.SetActive(false);

        if (cargoAtDestination != null)
            cargoAtDestination.SetActive(false);

        if (cargoOnTruckDay2 != null)
            cargoOnTruckDay2.SetActive(false);

        if (cargoAtDestinationDay2 != null)
            cargoAtDestinationDay2.SetActive(false);

        if (cargoOnTruckDay3 != null)
            cargoOnTruckDay3.SetActive(false);

        if (cargoAtDestinationDay3 != null)
            cargoAtDestinationDay3.SetActive(false);

        if (npc1 != null)
        {
            npc1.isFinished = false;
            if (npc1.questArrow != null)
                npc1.questArrow.SetActive(true);
        }

        UpdateQuestUI();
        Debug.Log("Đã bắt đầu chuyến hàng đợt 3");
    }

    public void MarkAllMissionComplete()
    {
        state = QuestState.Reported;

        if (vehicleArrow != null)
            vehicleArrow.ClearTarget();

        if (cargoOnTruck != null)
            cargoOnTruck.SetActive(false);

        if (cargoAtDestination != null)
            cargoAtDestination.SetActive(false);

        if (cargoOnTruckDay2 != null)
            cargoOnTruckDay2.SetActive(false);

        if (cargoAtDestinationDay2 != null)
            cargoAtDestinationDay2.SetActive(false);

        if (cargoOnTruckDay3 != null)
            cargoOnTruckDay3.SetActive(false);

        if (cargoAtDestinationDay3 != null)
            cargoAtDestinationDay3.SetActive(false);

        if (npc1 != null && npc1.questArrow != null)
            npc1.questArrow.SetActive(false);

        if (npc4 != null && npc4.questArrow != null)
            npc4.questArrow.SetActive(false);

        UpdateQuestUI();
        Debug.Log("Đã hoàn thành toàn bộ 3 đợt nhiệm vụ");
    }

    public void UpdateQuestUI()
    {
        if (QuestUIManager.Instance == null) return;

        if (currentDay == 1)
        {
            switch (state)
            {
                case QuestState.NotStarted:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 1: Đến gặp Trạm Trưởng Lâm");
                    break;

                case QuestState.InProgress:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 1: Lái xe đến gặp Trung Đội Trưởng Hoàng");
                    break;

                case QuestState.Completed:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 1: Quay về báo cáo cho Trạm Trưởng Lâm");
                    break;

                case QuestState.Reported:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 1 hoàn thành");
                    break;
            }
        }
        else if (currentDay == 2)
        {
            switch (state)
            {
                case QuestState.NotStarted:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 2: Nhận lệnh từ Trạm Trưởng Lâm");
                    break;

                case QuestState.InProgress:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 2: Lái xe đến gặp Thiếu Úy Sơn");
                    break;

                case QuestState.Completed:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 2: Quay về báo cáo với Trạm Trưởng Lâm");
                    break;

                case QuestState.Reported:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 2 hoàn thành");
                    break;
            }
        }
        else if (currentDay == 3)
        {
            switch (state)
            {
                case QuestState.NotStarted:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 3: Đến nhận lệnh từ Trạm Trưởng Lâm");
                    break;

                case QuestState.InProgress:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 3: Lái xe đến Hạ Sĩ Bảy");
                    break;

                case QuestState.Completed:
                    QuestUIManager.Instance.UpdateQuestText("Chuyến hàng 3: Quay về báo cáo với Trạm Trưởng Lâm");
                    break;

                case QuestState.Reported:
                    QuestUIManager.Instance.UpdateQuestText("Đã hoàn thành toàn bộ nhiệm vụ");
                    break;
            }
        }
    }
}