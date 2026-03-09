using UnityEngine;
using System.Collections;

public class TreeTrap : MonoBehaviour
{
    public Rigidbody tree;
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(FallTree());
        }
    }

    IEnumerator FallTree()
    {
        Quaternion startRot = tree.transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(-90, 0, 0);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            tree.transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }

        tree.isKinematic = true;
    }
}