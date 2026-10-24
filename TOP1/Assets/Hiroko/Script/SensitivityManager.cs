using UnityEngine;

public class SensitivityManager : MonoBehaviour
{
    public static float Sensitivity = 2.0f;

    private void Awake()
    {
        Sensitivity = PlayerPrefs.GetFloat(
            "Sensitivity", 2.0f
        );
    }

    public static void SetSensitivity(float value)
    {
        Sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }
}