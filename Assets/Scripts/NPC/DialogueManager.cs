using System.Collections;
using UnityEngine;
using TMPro;

// KẾT CẤU MỚI: Chứa cả Nội dung chữ và File ghi âm
[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 10)]
    public string sentenceText; // Ô để gõ chữ
    public AudioClip voiceClip; // Ô để kéo file mp3 vào
}

public class DialogueManager : MonoBehaviour
{
    [Header("Giao Diện UI (Kéo thả vào đây)")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject acceptButton;

    [Header("Âm Thanh Lồng Tiếng")]
    public AudioSource audioSource; // Kéo component Audio Source vào đây

    [Header("Cài đặt")]
    public float typingSpeed = 0.04f;

    private DialogueLine[] currentSentences; // Đã đổi sang mảng kiểu mới
    private int index;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    // Các biến lưu trữ
    private GameObject gameplayCamera;
    private GameObject currentDialogueCamera;
    private GameObject currentPlayer;
    private NPCInteract currentNPC; // Thêm dòng này để ghi nhớ NPC

    void Start()
    {
        dialoguePanel.SetActive(false);
        acceptButton.SetActive(false);
        gameplayCamera = Camera.main.gameObject;
    }

    void Update()
    {
        if (dialoguePanel.activeSelf)
        {
            // Bấm Space hoặc Chuột trái để qua câu nhanh
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (isTyping)
                {
                    if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                    dialogueText.text = currentSentences[index].sentenceText;
                    isTyping = false;

                    // MỚI: Nếu người chơi bấm tua nhanh, tắt tiếng ngay để không bị ồn
                    if (audioSource != null) audioSource.Stop();
                }
                else
                {
                    NextSentence();
                }
            }
        }
    }

    public void StartDialogue(string npcName, DialogueLine[] sentences, GameObject npcCamera, GameObject player, NPCInteract npc)
    {
        currentNPC = npc; // Lưu lại anh NPC đang giao tiếp

        nameText.text = npcName;
        currentSentences = sentences;
        index = 0;

        dialoguePanel.SetActive(true);
        acceptButton.SetActive(false);

        // Đổi máy quay
        currentDialogueCamera = npcCamera;
        if (gameplayCamera != null) gameplayCamera.SetActive(false);
        if (currentDialogueCamera != null) currentDialogueCamera.SetActive(true);

        // Tàng hình nhân vật chính
        currentPlayer = player;
        if (currentPlayer != null) currentPlayer.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        // MỚI: XỬ LÝ PHÁT ÂM THANH
        if (audioSource != null)
        {
            audioSource.Stop(); // Dừng tiếng câu trước (nếu đang nói dở)
            if (currentSentences[index].voiceClip != null) // Nếu bạn có kéo mp3 vào
            {
                audioSource.clip = currentSentences[index].voiceClip;
                audioSource.Play();
            }
        }

        foreach (char c in currentSentences[index].sentenceText.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void NextSentence()
    {
        if (index < currentSentences.Length - 1)
        {
            index++;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            acceptButton.SetActive(true);
        }
    }

    public void AcceptQuest()
    {
        // MỚI: Báo cho NPC biết là đã nhận lệnh xong để tắt mũi tên
        if (currentNPC != null)
        {
            currentNPC.CompleteInteraction();
        }

        dialoguePanel.SetActive(false);

        // Tắt tiếng khi đóng bảng thoại
        if (audioSource != null) audioSource.Stop();

        // Trả lại máy quay
        if (currentDialogueCamera != null) currentDialogueCamera.SetActive(false);
        if (gameplayCamera != null) gameplayCamera.SetActive(true);

        // Hiện lại nhân vật chính
        if (currentPlayer != null) currentPlayer.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}