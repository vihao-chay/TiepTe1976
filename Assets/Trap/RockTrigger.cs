using UnityEngine;

public class RockTrigger : MonoBehaviour
{
    public RollingRock rock;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rock.StartRolling();
        }
    }
}