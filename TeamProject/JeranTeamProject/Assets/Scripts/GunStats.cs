using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public GameObject gunModel;

    [Range(0.05f, 10f)] public float shootRate;
    [Range(1, 500)] public int magSizeMax;
    [Range(0.5f, 10)] public float reloadTime;
    public Bullet bullet;
    public AudioClip[] aud;

    public int currentAmmo;
    [Range(1, 500)] public int maxAmmo;
    AudioClip[] shotSound;
    [Range(0, 1)] public float shotSoundVol;

}
