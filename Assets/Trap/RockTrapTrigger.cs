using UnityEngine;

public class RockTrapTrigger : MonoBehaviour
{
    public Rigidbody rock;
    public GameObject rockDecoration;
    public float delay = 0.5f;

    void Start()
    {
        rock.gameObject.SetActive(false);      // ẩn đá rơi
        rockDecoration.SetActive(false);       // ẩn đá trang trí
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("ActivateTrap", delay);
        }
    }

    void ActivateTrap()
    {
        rockDecoration.SetActive(true);  // hiện đá trang trí
        rock.gameObject.SetActive(true); // hiện đá rơi
        rock.useGravity = true;
    }
}