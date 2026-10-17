
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("タイマー表示")]
    public TextMeshProUGUI timerText;

    [Header("ゲームオーバー画面")]
    public GameObject gameOverPanel;

    [Header("プレイヤー操作スクリプト")]
    public MonoBehaviour[] playerControls;

    [Header("カメラ")]
    public CameraFollow cameraFollow;

    [Header("制限時間（秒）")]
    public float timeLimit = 180f;

    [Header("タイトルシーン名")]
    public string titleSceneName = "TitleScene";

    private float timeRemaining;
    private bool isRunning = true;

    void Start()
    {
        timeRemaining = timeLimit;
        isRunning = true;

        // ゲームオーバー画面を非表示
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // タイムスケールを通常に戻す
        Time.timeScale = 1f;

        UpdateTimerText();
    }

    void Update()
    {
        if (!isRunning)
            return;

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
            return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void GameOver()
    {
        if (!isRunning)
            return;

        isRunning = false;
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

        // カメラ操作を停止
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

    // 再挑戦ボタン
    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    // タイトルに戻るボタン
    public void GoToTitle()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(titleSceneName);
    }
}
