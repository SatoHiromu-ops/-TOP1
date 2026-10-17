
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("自由移動設定")]
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderInterval = 3f;

    [Header("索敵設定")]
    [SerializeField] private float searchRange = 5f;
    [SerializeField] private float searchAngle = 90f;

    private Transform player;
    private NavMeshAgent agent;

    private bool isChasing = false;
    private float wanderTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 回転は自分で制御する
        agent.updateRotation = false;
        agent.updateUpAxis = true;

        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.isStopped = false;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        SetRandomDestination();
    }

    void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;

        // プレイヤーを発見するまでは索敵する
        if (!isChasing)
        {
            SearchForPlayer();

            if (!isChasing)
            {
                wanderTimer += Time.deltaTime;

                // 移動先に到着したら次の目的地を設定
                if (!agent.pathPending &&
                    (!agent.hasPath ||
                     agent.remainingDistance <=
                     agent.stoppingDistance + 0.2f))
                {
                    if (wanderTimer >= wanderInterval)
                    {
                        SetRandomDestination();
                    }
                }

                // 次の目的地を探す時間が過ぎた場合も再試行
                if (wanderTimer >= wanderInterval)
                {
                    SetRandomDestination();
                }

                return;
            }
        }

        // 発見したらプレイヤーを追い続ける
        if (player != null)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    void LateUpdate()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;

        // NavMeshAgent の実際の移動方向を取得
        Vector3 direction = agent.velocity;
        direction.y = 0f;

        // 移動中のみ回転させる
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(
                    direction.normalized,
                    Vector3.up
                );

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void SearchForPlayer()
    {
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance > searchRange)
            return;

        // 水平方向で索敵角度を判定
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        float angle = Vector3.Angle(
            transform.forward,
            direction
        );

        if (angle <= searchAngle / 2f)
        {
            isChasing = true;
        }
    }

    void SetRandomDestination()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;

        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPosition =
                transform.position +
                Random.insideUnitSphere * wanderRadius;

            if (NavMesh.SamplePosition(
                randomPosition,
                out NavMeshHit hit,
                wanderRadius,
                NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();

                if (agent.CalculatePath(hit.position, path) &&
                    path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.isStopped = false;
                    agent.SetDestination(hit.position);
                    wanderTimer = 0f;
                    return;
                }
            }
        }

        // 移動先が見つからなければ、次の Update で再試行
        wanderTimer = 0f;
    }

    void OnDrawGizmosSelected()
    {
        // 索敵距離
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRange);

        // 自由移動範囲
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
