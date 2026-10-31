using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        // ï€ë∂Ç≥ÇÍÇƒÇ¢ÇÈâπó Çì«Ç›çûÇﬁ
        float volume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        audioSource.volume = volume;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;

        // âπó Çï€ë∂Ç∑ÇÈ âπó ä«óù
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }
}