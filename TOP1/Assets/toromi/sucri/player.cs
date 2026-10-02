using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Transform cameraTransform;

    [Header("アイテム取得")]
    public float pickupDistance = 5f;
    private Item currentLookItem;

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

        if (input.sqrMagnitude < 0.01f)
            return;

        // カメラの前方向
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        // カメラの右方向
        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        // カメラ基準の移動方向
        Vector3 movement =
            cameraForward * input.y +
            cameraRight * input.x;

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

    // Fキーでアイテムを拾う
    void Update()
    {
        Item lookedItem = GetLookedItem();

        // 前に見ていたアイテム
        if (currentLookItem != null &&
            currentLookItem != lookedItem)
        {
            currentLookItem.SetHighlight(false);
        }

        // 今見ているアイテム
        if (lookedItem != null)
        {
            lookedItem.SetHighlight(true);
        }

        currentLookItem = lookedItem;

        // Fキーで拾う
        if (Keyboard.current != null &&
            Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (currentLookItem != null)
            {
                currentLookItem.Pickup();
                currentLookItem = null;
            }
        }
    }

    Item GetLookedItem()
    {
        // カメラの中心からRayを飛ばす
        Ray ray = new Ray(
            cameraTransform.position,
            cameraTransform.forward
        );

        RaycastHit hit;

        if (Physics.Raycast(
            ray,
            out hit,
            pickupDistance))
        {
            Item item = hit.collider.GetComponent<Item>();

            if (item != null)
            {
                return item;
            }
        }

        return null;
    }
}