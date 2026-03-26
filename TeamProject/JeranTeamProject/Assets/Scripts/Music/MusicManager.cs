using System.Collections;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioClip[] neutralTracks;
    [SerializeField] private AudioClip[] combatTracks;
    [SerializeField] private AudioClip[] bossTracks;

    [SerializeField] private float fadeDuration = 1f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayRandomNeutral();
    }
    public void PlayRandomNeutral()
    {
        PlayRandomTrackFromArray(neutralTracks);
    }
    public void PlayRandomCombat()
    {
        PlayRandomTrackFromArray(combatTracks);
    }
    public void PlayRandomBoss()
    {
        PlayRandomTrackFromArray(bossTracks);
    }
    private void PlayRandomTrackFromArray(AudioClip[] trackArray)
    {
        if (trackArray == null || trackArray.Length == 0)
        {
           // Debug.LogWarning("Track array is empty or null.");
            return;
        }
        int randomIndex = Random.Range(0, trackArray.Length);
        AudioClip chosenTrack = trackArray[randomIndex];

        PlayMusic(chosenTrack);
    }
    public void PlayMusic(AudioClip newTrack)
    {
        if (musicSource == null)
        {
           // Debug.LogWarning("Music source is not assigned.");
            return;
        }
        if (newTrack == null)
        {
            //Debug.LogWarning("New track is null.");
            return;
        }

      //  Debug.Log(" Tryng to play track: " + newTrack.name);
        if (musicSource.clip == newTrack)
        {
            //Debug.Log("Already playing this track.");
            return; 
        }
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeToNewTrack(newTrack));
    }

    private IEnumerator FadeToNewTrack(AudioClip newTrack)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newTrack;
        musicSource.loop = true;
        musicSource.Play();

        while (musicSource.volume < startVolume)
        {
            musicSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = startVolume;
    }

}
