using UnityEngine;

public class FinishCheck : MonoBehaviour
{
    public GameObject wall;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarMovement>() != null)
        {
            wall.SetActive(false);
        }
    }
}