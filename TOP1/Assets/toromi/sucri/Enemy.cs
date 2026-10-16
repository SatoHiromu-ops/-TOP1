using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("索敵設定")]
    [SerializeField] private float searchRange = 5f;  // 索敵距離
    [SerializeField] private float searchAngle = 90f; // 索敵角度

    private Transform player;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // プレイヤーへの方向と距離を取得
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        // 索敵範囲内かチェック
        if (distance <= searchRange)
        {
            // 敵の前方とプレイヤーの方向の角度を取得
            float angle = Vector3.Angle(transform.forward, direction);

            // 索敵角度内にプレイヤーがいる場合、追いかける
            if (angle <= searchAngle / 2f)
            {
                Vector3 moveDirection = direction.normalized;

                transform.position +=
                    moveDirection * moveSpeed * Time.deltaTime;
            }
        }
    }

    // 索敵範囲をSceneビューに表示
    void OnDrawGizmosSelected()
    {
        // 索敵範囲の色（半透明の黄色）
        Gizmos.color = new Color(1f, 1f, 0f, 0.25f);

        int segments = 30;
        float halfAngle = searchAngle / 2f;

        Vector3 origin = transform.position;

        // 扇形の中心方向
        Vector3 forward = transform.forward;

        // 扇形の三角形を描画
        for (int i = 0; i < segments; i++)
        {
            float angle1 = -halfAngle + searchAngle * i / segments;
            float angle2 = -halfAngle + searchAngle * (i + 1) / segments;

            Vector3 dir1 = Quaternion.Euler(0f, angle1, 0f) * forward;
            Vector3 dir2 = Quaternion.Euler(0f, angle2, 0f) * forward;

            Vector3 p1 = origin + dir1 * searchRange;
            Vector3 p2 = origin + dir2 * searchRange;

            Gizmos.DrawLine(origin, p1);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, origin);
        }

        // 索敵範囲の外周を赤色で表示
        Gizmos.color = Color.red;

        Vector3 left = Quaternion.Euler(0f, -halfAngle, 0f) * forward;
        Vector3 right = Quaternion.Euler(0f, halfAngle, 0f) * forward;

        Gizmos.DrawRay(origin, left * searchRange);
        Gizmos.DrawRay(origin, right * searchRange);
    }
}