using System;
using UnityEngine;
using System.Collections.Generic;

public enum GunRarity
{
    Common,
    Uncommon,
    Rare,
    Perfected,
    Exotic,
    Special
}

[CreateAssetMenu]
public class GunStats : ScriptableObject
{
    [Header("Objects")]
    public GameObject gunModel;
    public GameObject shootPos;
    public RecoilScriptable recoil;
    public Bullet bullet;
    public List<Attachments> attachments;

    [Header("Shooting")]
    [Range(0.05f, 10f)] public float shootRate;
    [Range(1, 500)] public int magSizeMax;
    [Range(0.5f, 10)] public float reloadTime;
    public int currentAmmo;

    [Header("Other")]
    public GunRarity gunRarity;
    public int maxAttachments;

    [Header("Positioning")]
    public Vector3 scale;
    public Vector3 position;
    public Quaternion rotation;
    public Quaternion shootRotate;


    [Header("Aiming")]
    public float hipSpread;
    public float adsSpread;

    public float hipX;
    public float hipY;
    public float hipZ;
    public float adsX;
    public float adsY;
    public float adsZ;

    public float adsZoom;

    [Header("Shotgun/Burst Variables")]
    public bool isShotgun;
    public bool isBurst;
    public float burstTime;
    public float burstDelay;
    public float rechamberTime;
    public int burstAmount;
    public int pelletAmount;

    [Header("Audio")]
    public AudioClip[] aud;
    AudioClip[] shotSound;
    [Range(0, 1)] public float shotSoundVol;


}
