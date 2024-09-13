using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; 
    [SerializeField] private Text volumeText;     

    private void Start()
    {
        volumeSlider.value = AudioListener.volume;

        volumeText.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";

        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
    }

    public void OnVolumeChanged()
    {
        AudioListener.volume = volumeSlider.value;

        volumeText.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";
    }
}
