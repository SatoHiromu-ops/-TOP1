using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Transform cameraTransform;

    [Header("アイテム取得")]
    public float pickupDistance = 5f;
    private Item currentLookItem;
    public Transform holdPoint;

    [Header("懐中電灯")]
    public GameObject flashlight;
    public Light flashlightLight;

    private bool flashlightIsHeld = false;
    private bool flashlightIsOn = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Playerが横転しないようにする
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        // Main Cameraを取得
        cameraTransform = Camera.main.transform;

        // 最初は懐中電灯をしまっておく
        flashlight.SetActive(false);

        // 最初はライトOFF
        flashlightLight.enabled = false;
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

    void Update()
    {
        // =========================
        // Eキー：懐中電灯を出す・しまう
        // =========================
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleFlashlight();
        }

        // =========================
        // 右クリック：ライトON/OFF
        // =========================
        if (Mouse.current != null &&
            Mouse.current.rightButton.wasPressedThisFrame)
        {
            ToggleFlashlightLight();
        }
    }

    // ========================================
    // 懐中電灯を出す / しまう
    // ========================================
    void ToggleFlashlight()
    {
        flashlightIsHeld = !flashlightIsHeld;

        if (flashlightIsHeld)
        {
            // 懐中電灯を表示
            flashlight.SetActive(true);

            // 手の位置にする
            flashlight.transform.SetParent(holdPoint);

            flashlight.transform.localPosition = Vector3.zero;
            flashlight.transform.localRotation = Quaternion.identity;

            // 最初はライトOFF
            flashlightIsOn = false;
            flashlightLight.enabled = false;
        }
        else
        {
            // ライトをOFF
            flashlightIsOn = false;
            flashlightLight.enabled = false;

            // 懐中電灯を非表示
            flashlight.SetActive(false);

            // 親から外す
            flashlight.transform.SetParent(null);
        }
    }

    // ========================================
    // 懐中電灯のライトON / OFF
    // ========================================
    void ToggleFlashlightLight()
    {
        // 懐中電灯を持っていないなら何もしない
        if (!flashlightIsHeld)
            return;

        flashlightIsOn = !flashlightIsOn;

        flashlightLight.enabled = flashlightIsOn;
    }
}