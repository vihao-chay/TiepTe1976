using UnityEngine;

public class move : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    private float moveSpeed = 5f;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        _characterController.Move(move * moveSpeed * Time.deltaTime);
    }
}