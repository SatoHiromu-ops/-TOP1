
using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    [Header("点滅設定")]
    public float minInterval = 0.1f;
    public float maxInterval = 2.0f;

    [Header("明るさ")]
    public float minAlpha = 0f;
    public float maxAlpha = 1f;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            // 不規則な待ち時間
            yield return new WaitForSeconds(
                Random.Range(minInterval, maxInterval)
            );

            // 一瞬消える
            canvasGroup.alpha = minAlpha;

            yield return new WaitForSeconds(
                Random.Range(0.03f, 0.15f)
            );

            // 点灯
            canvasGroup.alpha = maxAlpha;

            // たまに連続して点滅
            if (Random.value < 0.3f)
            {
                yield return new WaitForSeconds(0.08f);
                canvasGroup.alpha = minAlpha;

                yield return new WaitForSeconds(0.06f);
                canvasGroup.alpha = maxAlpha;
            }
        }
    }
}
