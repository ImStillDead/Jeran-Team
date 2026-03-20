using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioSettingsController : MonoBehaviour
{
    [SerializeField] Slider soundsSlider;
    [SerializeField] AudioMixer masterMixer;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("SavedMasterVolume", 1f);
        SetVolume(savedVolume);
    }

    public void SetVolume(float value)
    {
        float volumeFix = Mathf.Log10(value / 100) * 20f;
        if (value < 1) { volumeFix = -80; }

        RefreshSlider(value);

        PlayerPrefs.SetFloat("SavedMasterVolume", value);
        masterMixer.SetFloat("Master", volumeFix);
    }

    public void SetVolumeFromSlider()
    {
        if (soundsSlider != null)
            SetVolume(soundsSlider.value);
    }

    public void RefreshSlider(float value)
    {
        if (soundsSlider != null)
            soundsSlider.value = value;
    }
}
