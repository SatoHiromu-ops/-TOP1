using UnityEngine;

public class Item : MonoBehaviour
{
    private Renderer itemRenderer;
    private Color originalColor;

    [Header("強調表示")]
    public Color highlightColor = Color.yellow;

    void Start()
    {
        itemRenderer = GetComponent<Renderer>();

        if (itemRenderer != null)
        {
            originalColor = itemRenderer.material.color;
        }
    }

    // 強調表示ON
    public void SetHighlight(bool highlight)
    {
        if (itemRenderer == null)
            return;

        if (highlight)
        {
            itemRenderer.material.color = highlightColor;
        }
        else
        {
            itemRenderer.material.color = originalColor;
        }
    }

    // アイテムを拾う
    public void Pickup()
    {
        Debug.Log("アイテムを取得！");

        Destroy(gameObject);
    }
}