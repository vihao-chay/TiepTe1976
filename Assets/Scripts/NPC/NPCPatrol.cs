using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    [Header("Lộ Trình Đi Tuần")]
    public Transform[] waypoints; // Kéo Điểm_A và Điểm_B vào đây
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private int currentPoint = 0;
    private bool isWaiting = false; // Cờ đánh dấu đang đứng nghỉ

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // CỰC KỲ QUAN TRỌNG: Tắt thắng gấp để NPC không bị kẹt cứng khi đến đích
        agent.autoBraking = false;

        if (waypoints.Length > 0)
        {
            GotoNextPoint();
        }
    }

    void GotoNextPoint()
    {
        agent.isStopped = false; // Mở khóa cho phép di chuyển lại
        agent.SetDestination(waypoints[currentPoint].position); // Chỉ định đích đến

        // Tính toán điểm tiếp theo: Đang là A(0) thì sang B(1), đang là B(1) thì quay về A(0)
        currentPoint = (currentPoint + 1) % waypoints.Length;
    }

    void Update()
    {
        // Nếu không có điểm đến thì bỏ qua
        if (waypoints.Length == 0) return;

        // Nếu đã đến gần đích (cách 1.5m) và đang KHÔNG ở trạng thái chờ
        if (!agent.pathPending && agent.remainingDistance <= 1.5f && !isWaiting)
        {
            // Bắt đầu quy trình đứng nghỉ ngơi
            StartCoroutine(WaitRoutine());
        }
    }

    // Hàm quy trình nghỉ ngơi độc lập (Bất chấp giật lag)
    IEnumerator WaitRoutine()
    {
        isWaiting = true; // Bật cờ báo hiệu đang nghỉ
        agent.isStopped = true; // Bắt buộc NPC đứng im tại chỗ

        // Tạm dừng đoạn code này trong đúng số giây bạn cài (waitTime)
        yield return new WaitForSeconds(waitTime);

        isWaiting = false; // Tắt cờ nghỉ ngơi
        GotoNextPoint(); // Ra lệnh đi tới điểm tiếp theo
    }
}