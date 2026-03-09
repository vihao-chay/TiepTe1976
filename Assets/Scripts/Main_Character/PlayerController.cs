using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private Animator anim;
    private Transform cameraTransform;
    private bool isGrounded;

    private Vector3 moveDirection;
    private float currentSpeed;
    private bool isJumping;

    // LƯU HƯỚNG XOAY CUỐI
    private float lastRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;

        lastRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        // Luôn cập nhật camera
        cameraTransform = Camera.main.transform;

        // Kiểm tra chạm đất
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isRunning = Input.GetMouseButton(1);
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Nếu có input di chuyển
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle =
                Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
                + cameraTransform.eulerAngles.y;

            // Lưu hướng xoay
            lastRotation = targetAngle;

            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            anim.SetFloat("Speed", isRunning ? 1f : 0.5f);
        }
        else
        {
            // Không di chuyển → giữ hướng cũ
            moveDirection = Vector3.zero;

            transform.rotation = Quaternion.Euler(0f, lastRotation, 0f);

            anim.SetFloat("Speed", 0f);
        }

        // Nhảy
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = moveDirection * currentSpeed;

        // Giữ lực rơi
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;

        // Nhảy
        if (isJumping)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            anim.SetTrigger("Jump");

            isJumping = false;
        }
    }
}