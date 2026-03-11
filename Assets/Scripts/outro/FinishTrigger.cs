using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private bool hasFinished = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasFinished) return;

        if (other.GetComponentInParent<CarMovement>() != null || other.CompareTag("Player"))
        {
            hasFinished = true;
            GameOutro outro = Object.FindFirstObjectByType<GameOutro>();
            if (outro != null)
            {
                // GỌI HÀM 1: Chỉ hiện chữ, vẫn trong game
                outro.ShowVictoryText("NHIỆM VỤ HOÀN THÀNH!\nHÃY ĐẾN GẶP CHỈ HUY ĐỂ BÁO CÁO.");
            }
        }
    }
}