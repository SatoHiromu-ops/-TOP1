using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    public Slider bgmSlider;

    void Start()
    {
        // 保存された音量をスライダーに反映
        float volume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        bgmSlider.value = volume;

        // スライダーの変更を音量に反映
        bgmSlider.onValueChanged.AddListener(
            BGMManager.Instance.SetVolume
        );
    }
}