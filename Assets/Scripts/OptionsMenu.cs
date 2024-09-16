using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;   
    [SerializeField] private Text musicVolumeText;       
    [SerializeField] private Slider sfxVolumeSlider;    
    [SerializeField] private Text sfxVolumeText;        

    private MusicManager musicManager;   
    private AudioSource[] allAudioSources; 

    private void Start()
    {

        musicManager = FindObjectOfType<MusicManager>();

        allAudioSources = FindObjectsOfType<AudioSource>();

        if (musicManager != null)
        {
            musicVolumeSlider.value = musicManager.musicVolume;
        }

        musicVolumeText.text = Mathf.RoundToInt(musicVolumeSlider.value * 100) + "%";

        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });

        sfxVolumeSlider.value = AudioListener.volume;

        sfxVolumeText.text = Mathf.RoundToInt(sfxVolumeSlider.value * 100) + "%";

        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChanged(); });
    }

    public void OnMusicVolumeChanged()
    {
        if (musicManager != null)
        {
            musicManager.SetMusicVolume(musicVolumeSlider.value);
            musicVolumeText.text = Mathf.RoundToInt(musicVolumeSlider.value * 100) + "%";
        }
    }

    public void OnSFXVolumeChanged()
    {
        AudioListener.volume = sfxVolumeSlider.value;

        sfxVolumeText.text = Mathf.RoundToInt(sfxVolumeSlider.value * 100) + "%";
    }
}
