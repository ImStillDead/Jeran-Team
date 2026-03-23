using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public enum MusicType
    {
        Neutral,
        Combat,
        Boss
    }

    [SerializeField] private MusicType musicType;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger: " + other.name);
        Debug.Log("Other tag: " + other.tag);
        Debug.Log("Root name: " + other.transform.root.name);
        Debug.Log("Root tag: " + other.transform.root.tag);

        if (MusicManager.instance == null)
        {
            Debug.LogWarning("MusicManager instance is missing.");
            return;
        }
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            Debug.Log("Player entered music trigger: " + musicType);

            switch (musicType)
            {
                case MusicType.Neutral:
                    MusicManager.instance.PlayRandomNeutral();
                    break;
                case MusicType.Combat:
                    MusicManager.instance.PlayRandomCombat();
                    break;
                case MusicType.Boss:
                    MusicManager.instance.PlayRandomBoss();
                    break;
            }
        }
    }
}
