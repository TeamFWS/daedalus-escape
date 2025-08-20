using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private const float DefaultVolume = 80f;

    private static MusicPlayer _instance;
    [SerializeField] private string volumeKey = "musicVolume";
    [SerializeField] private string muteKey = "muteStatus";
    private AudioSource _audioSource;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();

        if (_audioSource != null)
        {
            _audioSource.Play();
            _audioSource.volume = PlayerPrefs.GetFloat(volumeKey, DefaultVolume) / 100f;
            _audioSource.mute = PlayerPrefs.GetInt(muteKey, 0) != 0;
        }
        else
        {
            Debug.LogError("AudioSource not found!");
        }
    }
}