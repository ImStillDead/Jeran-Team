using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip neutralTrack;

    void Start()
    {
        Debug.Log("MusicManager Start ran.");
        PlayNeutralMusic();
    }

    void PlayNeutralMusic()
    {
        if (musicSource == null)
        {
            Debug.LogWarning("musicSource is missing.");
            return;
        }

        if (neutralTrack == null)
        {
            Debug.LogWarning("neutralTrack is missing.");
            return;
        }

        musicSource.clip = neutralTrack;
        musicSource.loop = true;
        musicSource.Play();

        Debug.Log("Playing neutral music: " + neutralTrack.name);
    }
}
