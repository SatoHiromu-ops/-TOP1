
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("パネル表示")]
    public GameObject itemPanel;

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

        // アイテム取得数を増やす
        GameManager.instance.AddItem();

        // パネルを非表示
        if (itemPanel != null)
        {
            itemPanel.SetActive(false);
        }

        Destroy(gameObject);
    }
}
