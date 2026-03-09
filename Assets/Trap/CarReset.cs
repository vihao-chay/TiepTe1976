using System.Collections.Generic;
using UnityEngine;

public class CarReset : MonoBehaviour
{
    public float recordTime = 10f;

    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RecordPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCar();
        }
    }

    void RecordPosition()
    {
        if (positions.Count > recordTime * 50)
        {
            positions.RemoveAt(0);
            rotations.RemoveAt(0);
        }

        positions.Add(transform.position);
        rotations.Add(transform.rotation);
    }

    void ResetCar()
    {
        if (positions.Count > 0)
        {
            int index = 0; // vị trí cũ nhất (khoảng 5 giây trước)

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.position = positions[index];
            transform.rotation = rotations[index];
        }
    }
}