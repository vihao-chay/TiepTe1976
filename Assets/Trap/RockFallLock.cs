using UnityEngine;

public class RockFallLock : MonoBehaviour
{
    Rigidbody rb;
    bool locked = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!locked && collision.gameObject.GetComponent<Terrain>())
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            locked = true;
        }
    }
}