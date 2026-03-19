using UnityEngine;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    public static QuestUIManager Instance;

    public TextMeshProUGUI questText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateQuestText(string newText)
    {
        if (questText != null)
        {
            questText.text = newText;
        }
    }
}