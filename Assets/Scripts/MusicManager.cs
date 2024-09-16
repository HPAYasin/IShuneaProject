using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;   // Источник звука
    public AudioClip[] musicTracks;   // Массив музыкальных треков
    public float musicVolume = 1.0f;  // Громкость музыки

    private int currentTrackIndex = 0;

    private static MusicManager instance;

    private void Awake()
    {
        // Проверка на наличие другого MusicManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Сохраняем объект при переходе между сценами
        }
        else
        {
            Destroy(gameObject);  // Уничтожаем дублирующиеся объекты
        }
    }

    private void Start()
    {
        PlayNextTrack();  // Начинаем проигрывать музыку
    }

    private void Update()
    {
        // Если музыка закончилась, запускаем следующий трек
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    private void PlayNextTrack()
    {
        if (musicTracks.Length == 0) return;  // Если нет треков, ничего не делаем

        // Устанавливаем текущий трек
        audioSource.clip = musicTracks[currentTrackIndex];
        audioSource.volume = musicVolume;
        audioSource.Play();

        // Переходим к следующему треку, а если все треки проиграны, начинаем заново
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
    }

    // Метод для изменения громкости музыки
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        audioSource.volume = musicVolume;  // Применяем новую громкость сразу
    }
}
