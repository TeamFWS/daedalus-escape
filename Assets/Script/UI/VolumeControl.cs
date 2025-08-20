using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    private const float DefaultVolume = 80f;
    private const int UnmutedValue = 0;
    private const int MutedValue = 1;

    [SerializeField] private string volumeKey = "musicVolume";
    [SerializeField] private string muteKey = "muteStatus";
    [SerializeField] private string audioSourceTag = "MusicPlayer";

    [SerializeField] private LocalizedString localizedVolumeText;
    [SerializeField] private LocalizedString localizedMutedText;

    private AudioSource _audioSource;

    private Toggle _muteToggle;
    private Slider _slider;
    private TextMeshProUGUI _sliderText;

    private void Start()
    {
        _slider = GetComponentInChildren<Slider>();
        _muteToggle = GetComponentInChildren<Toggle>();
        _sliderText = GetComponentInChildren<TextMeshProUGUI>();
        _audioSource = GameObject.FindWithTag(audioSourceTag).GetComponent<AudioSource>();

        InitializeVolume();
        InitializeMute();
        UpdateSliderText();

        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveAllListeners();
        _muteToggle.onValueChanged.RemoveAllListeners();
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void InitializeVolume()
    {
        if (!PlayerPrefs.HasKey(volumeKey)) PlayerPrefs.SetFloat(volumeKey, DefaultVolume);

        _slider.value = PlayerPrefs.GetFloat(volumeKey);
        UpdateAudioVolume();

        _slider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    private void UpdateVolume()
    {
        PlayerPrefs.SetFloat(volumeKey, _slider.value);
        UpdateAudioVolume();
        UpdateSliderText();
    }

    private void UpdateAudioVolume()
    {
        _audioSource.volume = _slider.value / 100f;
    }

    private void InitializeMute()
    {
        if (!PlayerPrefs.HasKey(muteKey)) PlayerPrefs.SetInt(muteKey, UnmutedValue);

        var isMuted = PlayerPrefs.GetInt(muteKey) == MutedValue;
        _muteToggle.isOn = isMuted;
        _audioSource.mute = isMuted;

        _muteToggle.onValueChanged.AddListener(delegate { UpdateMute(); });
    }

    private void UpdateMute()
    {
        var isMuted = _muteToggle.isOn;
        PlayerPrefs.SetInt(muteKey, isMuted ? MutedValue : UnmutedValue);

        _audioSource.mute = isMuted;
        UpdateSliderText();
    }

    private void UpdateSliderText()
    {
        if (_muteToggle.isOn)
        {
            localizedMutedText.StringChanged += OnMutedStringChanged;
            localizedMutedText.RefreshString();
        }
        else
        {
            localizedVolumeText.Arguments = new object[] { _slider.value };
            localizedVolumeText.StringChanged += OnVolumeStringChanged;
            localizedVolumeText.RefreshString();
        }
    }

    private void OnVolumeStringChanged(string localizedText)
    {
        _sliderText.text = localizedText;
        localizedVolumeText.StringChanged -= OnVolumeStringChanged;
    }

    private void OnMutedStringChanged(string localizedText)
    {
        _sliderText.text = localizedText;
        localizedMutedText.StringChanged -= OnMutedStringChanged;
    }

    private void OnLocaleChanged(Locale newLocale)
    {
        UpdateSliderText();
    }
}