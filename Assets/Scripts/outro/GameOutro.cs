using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOutro : MonoBehaviour
{
    public CanvasGroup outroCanvasGroup;
    public TextMeshProUGUI outroText;
    public string menuSceneName = "MainStory";
    public float fadeSpeed = 0.5f;

    // HÀM 1: Chỉ hiện chữ trắng (Vẫn ở trong game, không mờ đen, không chuyển cảnh)
    public void ShowVictoryText(string message)
    {
        outroText.text = message;
        outroText.gameObject.SetActive(true);

        outroCanvasGroup.alpha = 0;
        outroCanvasGroup.gameObject.SetActive(true);

        // THÊM DÒNG NÀY: Cho phép click chuột xuyên qua Panel tàng hình
        outroCanvasGroup.blocksRaycasts = false;
    }

    // HÀM 2: Mờ dần đen và về Menu (Dành cho nút Accept của NPC)
    public void StartFinalExit(string goodbyeMessage)
    {
        StartCoroutine(RunFadeOutro(goodbyeMessage));
    }

    IEnumerator RunFadeOutro(string message)
    {
        outroText.text = message;

        // 🚨 THÊM 2 DÒNG NÀY: Phải đánh thức Panel và Chữ dậy trước khi làm mờ!
        outroCanvasGroup.gameObject.SetActive(true);
        outroText.gameObject.SetActive(true);

        // Mờ dần màn hình cho đến đen kịt
        while (outroCanvasGroup.alpha < 1)
        {
            outroCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(menuSceneName);
    }
}