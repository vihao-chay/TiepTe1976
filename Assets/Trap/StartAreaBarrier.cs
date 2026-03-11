using UnityEngine;

public class StartAreaBarrier : MonoBehaviour
{
    public GameObject barrier;

    public void OpenBarrier()
    {
        barrier.SetActive(false);
    }
}