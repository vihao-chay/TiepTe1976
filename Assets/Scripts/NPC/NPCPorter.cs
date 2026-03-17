using UnityEngine;
using UnityEngine.AI;

public class NPCPorter : MonoBehaviour
{
    [Header("Mục tiêu di chuyển")]
    public Transform point_Kho; // Kéo PointA_Kho vào đây
    public Transform point_Gao; // Kéo PointB_Gao vào đây

    [Header("Đạo cụ & Hoạt ảnh")]
    public GameObject baoGaoTrenVai; // Kéo cái Bao gạo (đang nằm trong xương NPC) vào đây
    private Animator anim;
    private NavMeshAgent agent;

    private bool dangVacGao = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Mới vào game: Không có gạo, đi đến đống gạo
        baoGaoTrenVai.SetActive(false);
        anim.SetBool("isCarrying", false);
        agent.SetDestination(point_Gao.position);
    }

    void Update()
    {
        // Kiểm tra xem NPC đã đi tới đích chưa (cách đích dưới 0.5 mét)
        if (!agent.pathPending && agent.remainingDistance <= 0.2f)
        {
            if (dangVacGao)
            {
                // Đang vác gạo mà tới đích -> Tức là đã về Kho -> Vứt gạo xuống
                DropRice();
            }
            else
            {
                // Đi người không mà tới đích -> Tức là tới Đống gạo -> Vác gạo lên
                PickUpRice();
            }
        }
    }

    void PickUpRice()
    {
        dangVacGao = true;
        baoGaoTrenVai.SetActive(true);
        anim.SetBool("isCarrying", true);

        // MỚI: Vác nặng nên đi chậm lại (Tùy chỉnh số 0.8f này cho khớp với mắt nhìn của bạn)
        agent.speed = 0.8f;

        agent.SetDestination(point_Kho.position);
    }

    void DropRice()
    {
        dangVacGao = false;
        baoGaoTrenVai.SetActive(false);
        anim.SetBool("isCarrying", false);

        // MỚI: Bỏ bao gạo xuống người nhẹ đi nhanh hơn (Khớp với dáng đi Walk)
        agent.speed = 1.5f;

        agent.SetDestination(point_Gao.position);
    }
}