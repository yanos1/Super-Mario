using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider slider; 
    public AudioSource musicSource;

    void Start()
    {
        slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });

        slider.value = musicSource.volume;
    }

    void OnSliderValueChanged()
    {
        musicSource.volume = slider.value;
    }
}
