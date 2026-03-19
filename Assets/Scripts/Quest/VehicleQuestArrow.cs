using UnityEngine;

public class VehicleQuestArrow : MonoBehaviour
{
    [Header("Mục tiêu")]
    public Transform target;

    [Header("Hiển thị")]
    public GameObject arrowVisual;

    [Header("Căn chỉnh sprite/model")]
    public Vector3 rotationOffset = new Vector3(90f, 0f, 0f);

    private bool isDriving = false;

    private void Start()
    {
        UpdateVisibility();
    }

    private void Update()
    {
        if (!isDriving || target == null || arrowVisual == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion lookRot = Quaternion.LookRotation(direction);
        arrowVisual.transform.rotation = lookRot * Quaternion.Euler(rotationOffset);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        UpdateVisibility();
    }

    public void ClearTarget()
    {
        target = null;
        UpdateVisibility();
    }

    public void SetDrivingState(bool driving)
    {
        isDriving = driving;
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (arrowVisual != null)
        {
            arrowVisual.SetActive(isDriving && target != null);
        }
    }
}