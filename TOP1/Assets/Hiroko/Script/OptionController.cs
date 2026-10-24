using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sensitivitySlider;

    void Start()
    {
        // BGM音量
        float volume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        bgmSlider.value = volume;

        if (BGMManager.Instance != null)
        {
            bgmSlider.onValueChanged.AddListener(
                BGMManager.Instance.SetVolume
            );
        }

        // マウス感度
        float sensitivity =
            PlayerPrefs.GetFloat("Sensitivity", 2.0f);

        sensitivitySlider.value = sensitivity;

        sensitivitySlider.onValueChanged.AddListener(
            SensitivityManager.SetSensitivity
        );
    }
}