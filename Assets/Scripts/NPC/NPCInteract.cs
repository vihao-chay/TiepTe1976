using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public GameObject promptUI;
    public DialogueManager dialogueManager;

    [Header("Góc Quay & Dữ Liệu")]
    public string npcName = "Cán bộ";
    public GameObject dialogueCamera;
    public DialogueLine[] sentences;

    [Header("Nhiệm Vụ")]
    public GameObject questArrow; // Kéo vật thể QuestArrow vào đây
    public bool isFinished = false; // Đánh dấu đã nhận lệnh xong

    private bool playerInRange = false;
    private GameObject playerRef;

    void Start()
    {
        promptUI.SetActive(false);
        if (questArrow != null) questArrow.SetActive(true); // Bật mũi tên lúc mới vào game
    }

    void Update()
    {
        // Chỉ cho phép ấn F nếu chưa nói chuyện xong (!isFinished)
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !isFinished)
        {
            promptUI.SetActive(false);
            // Thêm chữ "this" ở cuối để báo cho DialogueManager biết anh NPC nào đang nói
            dialogueManager.StartDialogue(npcName, sentences, dialogueCamera, playerRef, this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Chỉ hiện chữ F nếu chưa nhận lệnh
        if (other.CompareTag("Player") && !isFinished)
        {
            playerInRange = true;
            playerRef = other.gameObject;
            promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerRef = null;
            promptUI.SetActive(false);
        }
    }

    // Hàm này sẽ được gọi khi bạn bấm nút "Nhận Lệnh"
    public void CompleteInteraction()
    {
        isFinished = true; // Đánh dấu là đã xong nhiệm vụ
        if (questArrow != null) questArrow.SetActive(false); // Ẩn mũi tên đỏ
        promptUI.SetActive(false); // Ẩn chữ F vĩnh viễn
    }
}