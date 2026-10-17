



using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("タイマー表示")]
    public TextMeshProUGUI timerText;

    [Header("ゲームオーバー画面")]
    public GameObject gameOverPanel;

    [Header("プレイヤー操作スクリプト")]
    public MonoBehaviour[] playerControls;

    [Header("カメラ追従スクリプト")]
    public CameraFollow cameraFollow;

    [Header("制限時間（秒）")]
    public float timeLimit = 180f;

    private float timeRemaining;
    private bool isRunning = true;

    void Start()
    {
        timeRemaining = timeLimit;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateTimerText();
    }

    void Update()
    {
        if (!isRunning)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(0f, timeRemaining);

        UpdateTimerText();

        if (timeRemaining <= 0f)
        {
            GameOver();
        }
    }

    void UpdateTimerText()
    {
        if (timerText == null)
        {
            return;
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void GameOver()
    {
        if (!isRunning)
        {
            return;
        }

        // タイマー停止
        isRunning = false;

        // 時間を00:00に固定
        timeRemaining = 0f;
        UpdateTimerText();

        // プレイヤー操作を停止
        foreach (MonoBehaviour control in playerControls)
        {
            if (control != null)
            {
                control.enabled = false;
            }
        }

        // カメラの追従を停止
        if (cameraFollow != null)
        {
            cameraFollow.StopFollowing();
        }

        // ゲームオーバー画面を表示
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Debug.Log("GAME OVER!");
    }
}
