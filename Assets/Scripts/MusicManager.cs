using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;   
    public AudioClip[] musicTracks;   
    public float musicVolume = 1.0f;  

    private int currentTrackIndex = 0;

    private static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        PlayNextTrack();  
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    private void PlayNextTrack()
    {
        if (musicTracks.Length == 0) return;  

        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.volume = musicVolume;
        audioSource.Play();

        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        audioSource.volume = musicVolume;  
    }
}
