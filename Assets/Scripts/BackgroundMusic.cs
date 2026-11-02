using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;

    [Header("Audio")]
    public AudioSource audioSource;

    private void Awake()
    {
        // Prevent duplicate music managers
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep music alive between scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}