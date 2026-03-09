using UnityEngine;

public class RollingRock : MonoBehaviour
{
    Rigidbody rb;
    public float force = 20000f;
    public float pushForce = 4000f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void StartRolling()
    {
        rb.isKinematic = false;
        rb.AddForce(transform.forward * force);
        Destroy(gameObject, 120f);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Rigidbody carRb = col.gameObject.GetComponent<Rigidbody>();

            if (carRb != null)
            {
                carRb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
            }
        }
    }
}