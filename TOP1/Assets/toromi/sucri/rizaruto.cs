using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }
}