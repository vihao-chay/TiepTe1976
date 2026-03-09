using UnityEngine;

public class MineTrap : MonoBehaviour
{
    public float upForce = 30000f;
    public float backForce = 25000f;
    public float disableTime = 2f;

    [Header("Explosion Effect")]
    public GameObject explosionEffect;

    [Header("Camera Shake")]
    public Transform cameraToShake;
    public float shakeDuration = 0.4f;
    public float shakeAmount = 0.25f;

    private Vector3 originalCamPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            CarMovement car = other.GetComponentInParent<CarMovement>();

            // Hất xe
            if (rb != null)
            {
                Vector3 backward = -rb.linearVelocity.normalized;
                Vector3 force = Vector3.up * upForce + backward * backForce;

                rb.AddForce(force, ForceMode.Impulse);
            }

            // Khóa điều khiển xe
            if (car != null)
            {
                car.DisableControl(disableTime);
            }

            // Tạo hiệu ứng lửa
            if (explosionEffect != null)
            {
                GameObject fx = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(fx, 2f);
            }

            // Rung camera
            if (cameraToShake != null)
            {
                originalCamPos = cameraToShake.localPosition;
                StartCoroutine(ShakeCamera());
            }

            // Xóa mìn
            Destroy(gameObject);
        }
    }

    System.Collections.IEnumerator ShakeCamera()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            cameraToShake.localPosition = originalCamPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraToShake.localPosition = originalCamPos;
    }
}