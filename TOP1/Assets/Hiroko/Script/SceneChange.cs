using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }
}