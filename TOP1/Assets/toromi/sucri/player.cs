using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Playerが横転しないようにする
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        // Main Cameraを取得
        cameraTransform = Camera.main.transform;
    }

    void FixedUpdate()
    {
        Vector2 input = Vector2.zero;

        // WASD入力
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed)
                input.x -= 1f;

            if (Keyboard.current.dKey.isPressed)
                input.x += 1f;

            if (Keyboard.current.sKey.isPressed)
                input.y -= 1f;

            if (Keyboard.current.wKey.isPressed)
                input.y += 1f;
        }

        // 入力がない場合
        if (input.sqrMagnitude < 0.01f)
            return;

        // カメラの前方向
        Vector3 cameraForward = cameraTransform.forward;

        // 上下方向を無視する
        cameraForward.y = 0f;
        cameraForward.Normalize();

        // カメラの右方向
        Vector3 cameraRight = cameraTransform.right;

        // 上下方向を無視する
        cameraRight.y = 0f;
        cameraRight.Normalize();

        // カメラ基準の移動方向
        Vector3 movement =
            cameraForward * input.y +
            cameraRight * input.x;

        // 斜め移動が速くならないようにする
        movement = Vector3.ClampMagnitude(movement, 1f);

        // Playerを移動
        Vector3 newPosition =
            rb.position + movement * speed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);

        // 移動方向を向く
        Quaternion targetRotation =
            Quaternion.LookRotation(movement);

        rb.MoveRotation(
            Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            )
        );
    }
}