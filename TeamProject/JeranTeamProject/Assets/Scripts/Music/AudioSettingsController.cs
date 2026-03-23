using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioSettingsController : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider  sfxSlider;

    [SerializeField] private AudioMixer masterMixer;

    private void Start()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat("SavedMasterVolume", 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SavedSFXVolume", 1f);

        if (savedMasterVolume <= 0.0001f)
            savedMasterVolume = 1f;

           if (savedSFXVolume <= 0.0001f)
               savedSFXVolume = 1f;

        if (masterSlider != null)
            masterSlider.value = savedMasterVolume;

        if (sfxSlider != null)
            sfxSlider.value = savedSFXVolume;

        SetMasterVolume(savedMasterVolume);
        SetSfxVolume(savedSFXVolume);
    }

    public void SetMasterVolume(float value)
    {
        if (value < 0.0001f)
            value = 0.0001f;

        PlayerPrefs.SetFloat("SavedMasterVolume", value);
        PlayerPrefs.Save();

        masterMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
    }
    public void SetSfxVolume(float value)
    {
        if (value < 0.0001f)
            value = 0.0001f;

        PlayerPrefs.SetFloat("SavedSFXVolume", value);
        PlayerPrefs.Save();

        masterMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
    }

    public void SetMasterVolumeFromSlider()
    {
        if (masterSlider != null)
            SetMasterVolume(masterSlider.value);
    }
    public void SetSfxVolumeFromSlider()
    {
        if (sfxSlider != null)
            SetSfxVolume(sfxSlider.value);
    }
}
