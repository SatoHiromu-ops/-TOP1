using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionButton : MonoBehaviour
{
    public void GoToOptionScene()
    {
        SceneManager.LoadScene("Option");
    }
}