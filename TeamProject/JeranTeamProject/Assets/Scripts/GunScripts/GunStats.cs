using System;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class GunStats : ScriptableObject
{
    public GameObject gunModel;
    public GameObject shootPos;

    [Range(0.05f, 10f)] public float shootRate;
    [Range(1, 500)] public int magSizeMax;
    [Range(0.5f, 10)] public float reloadTime;



    public Bullet bullet;
    public AudioClip[] aud;
    public Vector3 scale;
    public Vector3 postion;
    public Quaternion rotation;
    public Quaternion shootRotate;
    public int currentAmmo;

    public float spread;
   
    public bool isShotgun;
    public bool isBurst;
    public float burstTime;
    public float burstDelay;
    public float rechamberTime;
    public int burstAmount;
    public int pelletAmount;


    AudioClip[] shotSound;
    [Range(0, 1)] public float shotSoundVol;

}
