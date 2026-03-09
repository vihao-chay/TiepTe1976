using UnityEngine;

public class SpikeStick : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Terrain>() != null)
        {
            rb.isKinematic = true;
        }
    }
}