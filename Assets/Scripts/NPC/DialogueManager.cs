using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 10)]
    public string sentenceText;
    public AudioClip voiceClip;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Giao Diện UI (Kéo thả vào đây)")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject acceptButton;

    [Header("Âm Thanh Lồng Tiếng")]
    public AudioSource audioSource;

    [Header("Cài đặt")]
    public float typingSpeed = 0.04f;

    private DialogueLine[] currentSentences;
    private int index;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private GameObject gameplayCamera;
    private GameObject currentDialogueCamera;
    private GameObject currentPlayer;
    private NPCInteract currentNPC;

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (acceptButton != null) acceptButton.SetActive(false);

        if (Camera.main != null)
            gameplayCamera = Camera.main.gameObject;
    }

    void Update()
    {
        if (dialoguePanel != null && dialoguePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (isTyping)
                {
                    if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                    dialogueText.text = currentSentences[index].sentenceText;
                    isTyping = false;

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
        if (sentences == null || sentences.Length == 0)
        {
            Debug.LogWarning($"[{npcName}] không có câu thoại.");
            return;
        }

        currentNPC = npc;

        // --- MỚI: Tắt tiếng môi trường khi bắt đầu nói chuyện (Ấn F) ---
        AudioListener.pause = true;
        if (audioSource != null)
        {
            audioSource.ignoreListenerPause = true; // Cho phép loa NPC vẫn phát tiếng
        }
        // ---------------------------------------------------------------

        nameText.text = npcName;
        currentSentences = sentences;
        index = 0;
        dialogueText.text = "";

        dialoguePanel.SetActive(true);
        acceptButton.SetActive(false);

        currentDialogueCamera = npcCamera;
        if (gameplayCamera != null) gameplayCamera.SetActive(false);
        if (currentDialogueCamera != null) currentDialogueCamera.SetActive(true);

        currentPlayer = player;
        if (currentPlayer != null) currentPlayer.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        if (audioSource != null)
        {
            audioSource.Stop();
            if (currentSentences[index].voiceClip != null)
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
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

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
        if (currentNPC != null)
        {
            currentNPC.CompleteInteraction();
        }

        // --- MỚI: Mở lại tiếng môi trường khi kết thúc nói chuyện ---
        AudioListener.pause = false;
        // ------------------------------------------------------------

        dialoguePanel.SetActive(false);

        if (audioSource != null)
            audioSource.Stop();

        if (currentDialogueCamera != null)
            currentDialogueCamera.SetActive(false);

        if (gameplayCamera != null)
            gameplayCamera.SetActive(true);

        if (currentPlayer != null)
            currentPlayer.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}