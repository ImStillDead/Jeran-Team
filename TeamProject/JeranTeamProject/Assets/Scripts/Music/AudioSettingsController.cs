using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioSettingsController : MonoBehaviour
{
    [SerializeField] Slider soundsSlider;
    [SerializeField] AudioMixer masterMixer;

    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }

    public void SetVolume(float value)
    {
        if (value < 1)
            value = 0.001f;

        RefreshSlider(value);

        PlayerPrefs.SetFloat("SavedMasterVolume", value);

        masterMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundsSlider.value);
    }

    public void RefreshSlider(float value)
    {
        soundsSlider.value = value;
    }
}
