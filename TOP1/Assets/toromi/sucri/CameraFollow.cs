using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("三人称設定")]
    public float thirdPersonDistance = 6f;
    public float thirdPersonHeight = 2f;

    [Header("一人称設定")]
    public float firstPersonHeight = 1.6f;

    [Header("カメラ設定")]
    public float smoothSpeed = 10f;
    public float mouseSensitivity = 2f;

    [Header("上下回転")]
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;

    private float horizontalAngle = 0f;
    private float verticalAngle = 20f;

    // true = 三人称
    // false = 一人称
    private bool isThirdPerson = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.zKey.wasPressedThisFrame)
        {
            isThirdPerson = !isThirdPerson;
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // -------------------------
        // マウス入力
        // -------------------------

        Vector2 mouseInput = Vector2.zero;

        if (Mouse.current != null)
        {
            mouseInput = Mouse.current.delta.ReadValue();
        }

        horizontalAngle += mouseInput.x * mouseSensitivity;
        verticalAngle -= mouseInput.y * mouseSensitivity;

        verticalAngle = Mathf.Clamp(
            verticalAngle,
            minVerticalAngle,
            maxVerticalAngle
        );

        // -------------------------
        // カメラ回転
        // -------------------------

        Quaternion rotation = Quaternion.Euler(
            verticalAngle,
            horizontalAngle,
            0f
        );

        // -------------------------
        // 三人称
        // -------------------------

        if (isThirdPerson)
        {
            Vector3 targetPosition =
                target.position +
                Vector3.up * thirdPersonHeight;

            Vector3 cameraPosition =
                targetPosition -
                rotation * Vector3.forward *
                thirdPersonDistance;

            transform.position = Vector3.Lerp(
                transform.position,
                cameraPosition,
                smoothSpeed * Time.deltaTime
            );

            transform.LookAt(targetPosition);
        }

        // -------------------------
        // 一人称
        // -------------------------

        else
        {
            Vector3 firstPersonPosition =
                target.position +
                Vector3.up * firstPersonHeight;

            // 一人称では直接目の位置へ
            transform.position = firstPersonPosition;

            transform.rotation = rotation;
        }
    }
}