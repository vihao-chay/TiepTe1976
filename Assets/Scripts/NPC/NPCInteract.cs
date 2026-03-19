using UnityEngine;

public enum NPCActionType
{
    NormalTalk,
    GiveQuest,
    CompleteQuest
}

public class NPCInteract : MonoBehaviour
{
    public GameObject promptUI;
    public DialogueManager dialogueManager;

    [Header("Góc Quay & Dữ Liệu")]
    public string npcName = "Cán bộ";
    public GameObject dialogueCamera;

    [Header("Thoại thường / NPC giao hàng")]
    public DialogueLine[] sentences;

    [Header("Thoại báo cáo mặc định")]
    public DialogueLine[] reportSentences;

    [Header("Thoại riêng cho NPC1 theo từng đợt")]
    public DialogueLine[] day1Sentences;
    public DialogueLine[] day2Sentences;
    public DialogueLine[] day3Sentences;

    [Header("Thoại báo cáo riêng cho NPC1 theo từng đợt")]
    public DialogueLine[] day1ReportSentences;
    public DialogueLine[] day2ReportSentences;
    public DialogueLine[] day3ReportSentences;

    [Header("Loại hành động")]
    public NPCActionType actionType = NPCActionType.NormalTalk;

    [Header("Nhiệm Vụ")]
    public GameObject questArrow;
    public bool isFinished = false;

    [Header("Kết thúc game")]
    public string finalOutroMessage = "Tạm biệt chiến sĩ, hẹn gặp lại!";

    private bool playerInRange = false;
    private GameObject playerRef;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);

        if (questArrow != null)
        {
            if (actionType == NPCActionType.GiveQuest)
                questArrow.SetActive(!isFinished);
            else
                questArrow.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !isFinished)
        {
            if (promptUI != null)
                promptUI.SetActive(false);

            if (dialogueManager != null)
            {
                DialogueLine[] linesToUse = GetCurrentDialogueLines();
                dialogueManager.StartDialogue(npcName, linesToUse, dialogueCamera, playerRef, this);
            }
            else
            {
                Debug.LogWarning($"[{gameObject.name}] Chưa gán DialogueManager.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFinished)
        {
            playerInRange = true;
            playerRef = other.gameObject;

            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerRef = null;

            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }

    DialogueLine[] GetCurrentDialogueLines()
    {
        if (QuestManager.Instance == null)
            return sentences;

        if (actionType == NPCActionType.GiveQuest)
        {
            bool isReportDialogue = QuestManager.Instance.state == QuestState.Completed;

            if (isReportDialogue)
            {
                switch (QuestManager.Instance.currentDay)
                {
                    case 1:
                        if (day1ReportSentences != null && day1ReportSentences.Length > 0)
                            return day1ReportSentences;
                        break;

                    case 2:
                        if (day2ReportSentences != null && day2ReportSentences.Length > 0)
                            return day2ReportSentences;
                        break;

                    case 3:
                        if (day3ReportSentences != null && day3ReportSentences.Length > 0)
                            return day3ReportSentences;
                        break;
                }

                if (reportSentences != null && reportSentences.Length > 0)
                    return reportSentences;
            }
            else
            {
                switch (QuestManager.Instance.currentDay)
                {
                    case 1:
                        if (day1Sentences != null && day1Sentences.Length > 0)
                            return day1Sentences;
                        break;

                    case 2:
                        if (day2Sentences != null && day2Sentences.Length > 0)
                            return day2Sentences;
                        break;

                    case 3:
                        if (day3Sentences != null && day3Sentences.Length > 0)
                            return day3Sentences;
                        break;
                }

                if (sentences != null && sentences.Length > 0)
                    return sentences;
            }
        }

        if (QuestManager.Instance.state == QuestState.Completed &&
            reportSentences != null &&
            reportSentences.Length > 0)
        {
            return reportSentences;
        }

        return sentences;
    }

    public void CompleteInteraction()
    {
        if (QuestManager.Instance == null)
        {
            Debug.LogWarning("Chưa có QuestManager trong scene.");
            return;
        }

        switch (actionType)
        {
            case NPCActionType.GiveQuest:
                HandleGiveQuest();
                break;

            case NPCActionType.CompleteQuest:
                HandleCompleteQuest();
                break;

            case NPCActionType.NormalTalk:
                Debug.Log($"{npcName} chỉ nói chuyện bình thường");
                break;
        }

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void HandleGiveQuest()
    {
        if (QuestManager.Instance.state == QuestState.NotStarted)
        {
            QuestManager.Instance.state = QuestState.InProgress;

            NPCInteract currentTarget = QuestManager.Instance.GetCurrentTargetNPC();
            GameObject currentCargoOnTruck = QuestManager.Instance.GetCurrentCargoOnTruck();
            GameObject currentCargoAtDestination = QuestManager.Instance.GetCurrentCargoAtDestination();

            if (currentTarget != null && currentTarget.questArrow != null)
                currentTarget.questArrow.SetActive(true);

            if (QuestManager.Instance.vehicleArrow != null && currentTarget != null)
                QuestManager.Instance.vehicleArrow.SetTarget(currentTarget.transform);

            if (currentCargoOnTruck != null)
                currentCargoOnTruck.SetActive(true);

            if (currentCargoAtDestination != null)
                currentCargoAtDestination.SetActive(false);

            if (questArrow != null)
                questArrow.SetActive(false);

            QuestManager.Instance.UpdateQuestUI();

            Debug.Log($"{npcName}: Đã giao nhiệm vụ cho đợt {QuestManager.Instance.currentDay}");
        }
        else if (QuestManager.Instance.state == QuestState.Completed)
        {
            QuestManager.Instance.state = QuestState.Reported;
            QuestManager.Instance.UpdateQuestUI();

            if (questArrow != null)
                questArrow.SetActive(false);

            if (QuestManager.Instance.vehicleArrow != null)
                QuestManager.Instance.vehicleArrow.ClearTarget();

            GameObject currentCargoOnTruck = QuestManager.Instance.GetCurrentCargoOnTruck();
            GameObject currentCargoAtDestination = QuestManager.Instance.GetCurrentCargoAtDestination();

            if (currentCargoOnTruck != null)
                currentCargoOnTruck.SetActive(false);

            if (currentCargoAtDestination != null)
                currentCargoAtDestination.SetActive(false);

            if (QuestManager.Instance.currentDay == 1)
            {
                QuestManager.Instance.StartSecondTrip();
            }
            else if (QuestManager.Instance.currentDay == 2)
            {
                QuestManager.Instance.StartThirdTrip();
            }
            else if (QuestManager.Instance.currentDay == 3)
            {
                isFinished = true;
                QuestManager.Instance.MarkAllMissionComplete();

                GameOutro outro = FindFirstObjectByType<GameOutro>();
                if (outro != null)
                {
                    outro.StartFinalExit(finalOutroMessage);
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy GameOutro trong scene.");
                }

                Debug.Log("Đã hoàn thành toàn bộ 3 đợt nhiệm vụ");
            }
        }
    }

    void HandleCompleteQuest()
    {
        if (QuestManager.Instance.state == QuestState.InProgress)
        {
            NPCInteract currentTarget = QuestManager.Instance.GetCurrentTargetNPC();

            if (currentTarget != this)
            {
                Debug.Log($"{npcName}: Đây chưa phải điểm giao hàng hiện tại.");
                return;
            }

            QuestManager.Instance.state = QuestState.Completed;
            isFinished = true;

            GameObject currentCargoOnTruck = QuestManager.Instance.GetCurrentCargoOnTruck();
            GameObject currentCargoAtDestination = QuestManager.Instance.GetCurrentCargoAtDestination();

            if (currentCargoOnTruck != null)
                currentCargoOnTruck.SetActive(false);

            if (currentCargoAtDestination != null)
                currentCargoAtDestination.SetActive(true);

            if (questArrow != null)
                questArrow.SetActive(false);

            if (QuestManager.Instance.npc1 != null && QuestManager.Instance.npc1.questArrow != null)
                QuestManager.Instance.npc1.questArrow.SetActive(true);

            if (QuestManager.Instance.vehicleArrow != null && QuestManager.Instance.npc1 != null)
                QuestManager.Instance.vehicleArrow.SetTarget(QuestManager.Instance.npc1.transform);

            QuestManager.Instance.UpdateQuestUI();

            Debug.Log($"{npcName}: Đã hoàn thành điểm giao hàng cho đợt {QuestManager.Instance.currentDay}");
        }
    }
}