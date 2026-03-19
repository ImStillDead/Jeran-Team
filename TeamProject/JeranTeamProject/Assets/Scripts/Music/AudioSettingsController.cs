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
        if (value < 0.0001f)
            value = 0.0001f;

        RefreshSlider(value);

        PlayerPrefs.SetFloat("SavedMasterVolume", value);
        PlayerPrefs.Save();

        masterMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
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
