using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public StartAreaBarrier barrier;

    public void Interact()
    {
        Debug.Log("Player talked to NPC");

        if (barrier != null)
        {
            barrier.OpenBarrier();
        }
    }
}