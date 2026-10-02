using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Header("一人称設定")]
    public float firstPersonHeight = 1.6f;

    [Header("カメラ設定")]
    public float mouseSensitivity = 2f;

    [Header("上下回転")]
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;

    private float horizontalAngle = 0f;
    private float verticalAngle = 20f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // マウス入力
        Vector2 mouseInput = Vector2.zero;

        if (Mouse.current != null)
        {
            mouseInput = Mouse.current.delta.ReadValue();
        }

        // 左右回転
        horizontalAngle += mouseInput.x * mouseSensitivity;

        // 上下回転
        verticalAngle -= mouseInput.y * mouseSensitivity;

        // 上下の回転範囲を制限
        verticalAngle = Mathf.Clamp(
            verticalAngle,
            minVerticalAngle,
            maxVerticalAngle
        );
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // カメラの回転
        Quaternion rotation = Quaternion.Euler(
            verticalAngle,
            horizontalAngle,
            0f
        );

        // プレイヤーの目の高さ
        Vector3 firstPersonPosition =
            target.position +
            Vector3.up * firstPersonHeight;

        // カメラを目の位置に移動
        transform.position = firstPersonPosition;

        // カメラを回転
        transform.rotation = rotation;
    }
}