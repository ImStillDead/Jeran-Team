using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip neutralTrack;
    void Start()
    {
        PlayNeutralMusic();
    }

    // Update is called once per frame
    void PlayNeutralMusic()
    {
        if (musicSource != null && neutralTrack != null)
        {
            musicSource.clip = neutralTrack;
            musicSource.loop = true;
            musicSource.Play();
            Debug.Log("Music started playing.");
        }
        else
        {
            Debug.LogWarning("MusicSource or Track is missing.");
        }

    }
}
