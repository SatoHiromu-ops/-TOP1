using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

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

        // プレイヤーの方向を取得
        Vector3 direction = (player.position - transform.position).normalized;

        // プレイヤーに向かって移動
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
