using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("パネル表示")]
    public GameObject itemPanel;

    // パネル表示ON・OFF
    public void SetHighlight(bool highlight)
    {
        if (itemPanel == null)
            return;

        itemPanel.SetActive(highlight);
    }

    // アイテムを拾う
    public void Pickup()
    {
        Debug.Log("アイテムを取得！");

        // パネルを非表示
        if (itemPanel != null)
        {
            itemPanel.SetActive(false);
        }

        Destroy(gameObject);
    }
}