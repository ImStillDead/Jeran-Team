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
        float savedSfxVolume = PlayerPrefs.GetFloat("SavedSfxVolume", 1f);

        if (masterSlider != null)
            masterSlider.value = savedMasterVolume;

        if (sfxSlider != null)
            sfxSlider.value = savedSfxVolume;

        SetMasterVolume(savedMasterVolume);
        SetSfxVolume(savedSfxVolume);
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
        PlayerPrefs.SetFloat("SavedSfxVolume", value);
        PlayerPrefs.Save();
        masterMixer.SetFloat("SfxVolume", Mathf.Log10(value) * 20f);
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
