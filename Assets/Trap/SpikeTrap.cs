using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Rigidbody spike;
    public MeshRenderer spikeMesh;
    public float fallForce = 5000f;

    bool triggered = false;

    void Start()
    {
        // Ẩn cọc lúc đầu
        spikeMesh.enabled = false;

        // giữ cọc đứng yên trên trời
        spike.isKinematic = true;
        spike.useGravity = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;

            // hiện cọc
            spikeMesh.enabled = true;

            // bật vật lý
            spike.isKinematic = false;
            spike.useGravity = true;

            // tăng tốc rơi
            spike.AddForce(Vector3.down * fallForce);
        }
    }
}