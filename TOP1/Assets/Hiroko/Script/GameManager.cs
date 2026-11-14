
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int itemCount = 0;

    void Awake()
    {
        instance = this;
    }

    public void AddItem()
    {
        itemCount++;

        Debug.Log("アイテム取得数：" + itemCount);

        if (itemCount >= 3)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }
}
