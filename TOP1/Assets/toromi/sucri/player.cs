
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Transform cameraTransform;

    [Header("ダッシュ設定")]
    public float dashSpeed = 10f;

    [Header("スタミナ設定")]
    public float maxStamina = 100f;
    public float stamina = 100f;
    public float staminaUseRate = 25f;
    public float staminaRecoveryRate = 15f;

    [Header("スタミナゲージ")]
    public Slider staminaSlider;

    [Header("アイテム取得")]
    public float pickupDistance = 5f;
    private Item currentLookItem;

    [Header("懐中電灯")]
    public GameObject flashlight;
    public Transform holdPoint;

    [Header("Playerの光源")]
    public Light playerLight;

    private bool flashlightIsHeld = false;
    private bool flashlightIsOn = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        cameraTransform = Camera.main.transform;

        flashlight.SetActive(false);

        if (playerLight != null)
        {
            playerLight.enabled = false;
        }

        flashlightIsOn = false;

        playerLight.transform.SetParent(cameraTransform, false);
        playerLight.transform.localPosition = new Vector3(0f, 0f, 0.3f);
        playerLight.transform.localRotation = Quaternion.identity;

        // スタミナ初期化
        stamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = stamina;
        }
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

        // ダッシュ判定
        // ダッシュ判定
        // ダッシュ判定
        bool isDashing =
            Keyboard.current != null &&
            Keyboard.current.leftShiftKey.isPressed &&
            stamina > 0f &&
            input.sqrMagnitude > 0.01f;

        // 移動速度
        float currentSpeed = speed;

        if (isDashing)
        {
            currentSpeed = dashSpeed;

            stamina -= staminaUseRate * Time.fixedDeltaTime;

            // 0以下になったら即座に通常速度に戻す
            if (stamina <= 0f)
            {
                stamina = 0f;
                currentSpeed = speed;
            }
        }
        else
        {
            stamina += staminaRecoveryRate * Time.fixedDeltaTime;
        }

        // スタミナ制限
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);

        // ゲージ更新
        if (staminaSlider != null)
        {
            staminaSlider.value = stamina;
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

        // Playerを移動（ダッシュ対応）
        Vector3 newPosition =
            rb.position + movement * currentSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);

        // カメラの向いている水平方向を向く
        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(lookDirection);

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                )
            );
        }
    }

    void Update()
    {
        CheckLookItem();

        // Eキー：懐中電灯を出す・しまう
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleFlashlight();
        }

        // 右クリック：ライトON/OFF
        if (Mouse.current != null &&
            Mouse.current.rightButton.wasPressedThisFrame)
        {
            ToggleFlashlightLight();
        }

        // Fキー：アイテムを拾う
        if (Keyboard.current != null &&
            Keyboard.current.fKey.wasPressedThisFrame)
        {
            TryPickupItem();
        }
    }

    // 懐中電灯を出す / しまう
    void ToggleFlashlight()
    {
        flashlightIsHeld = !flashlightIsHeld;

        if (flashlightIsHeld)
        {
            flashlight.transform.SetParent(holdPoint, false);
            flashlight.transform.localPosition = Vector3.zero;
            flashlight.transform.localRotation = Quaternion.identity;
            flashlight.SetActive(true);

            flashlightIsOn = false;
            playerLight.enabled = false;
        }
        else
        {
            flashlightIsOn = false;
            playerLight.enabled = false;

            flashlight.SetActive(false);
            flashlight.transform.SetParent(null);
        }
    }

    // 懐中電灯のライトON / OFF
    void ToggleFlashlightLight()
    {
        if (!flashlightIsHeld)
            return;

        flashlightIsOn = !flashlightIsOn;
        playerLight.enabled = flashlightIsOn;
    }

    // Fキー：アイテムを拾う
    void TryPickupItem()
    {
        if (currentLookItem == null)
            return;

        Item item = currentLookItem;
        currentLookItem = null;

        item.Pickup();
    }

    void CheckLookItem()
    {
        Item foundItem = null;

        if (cameraTransform != null)
        {
            Ray ray = new Ray(
                cameraTransform.position,
                cameraTransform.forward
            );

            if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
            {
                foundItem = hit.collider.GetComponentInParent<Item>();
            }
        }

        if (currentLookItem != null && currentLookItem != foundItem)
        {
            currentLookItem.SetHighlight(false);
        }

        currentLookItem = foundItem;

        if (currentLookItem != null)
        {
            currentLookItem.SetHighlight(true);
        }
    }
}
