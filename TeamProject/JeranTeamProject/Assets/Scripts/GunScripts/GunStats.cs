using System;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public enum GunRarity
{
    Common,
    Uncommon,
    Rare,
    Exotic,
    Perfected,
    Special
}
public enum GunType
{
    Pistol,
    AR,
    LMG,
    Shotgun,
    SMG,
    Sniper,
    RocketLauncher
}

[CreateAssetMenu]
public class GunStats : ScriptableObject
{

    [Header("Dragged Stuff")]
    public GameObject gunModel;
    public GameObject shootPos;
    public RecoilScriptable recoil;
    public List<Attachment> attachmentList;
    public Bullet bullet;


    [Header("Gun Info")]
    [Range(0.05f, 10f)] public float shootRate;
    [Range(1, 500)] public int magSizeMax;
    [Range(0.5f, 10)] public float reloadTime;
    public int currentAmmo;
    public GunRarity gunRarity;
    public GunType gunType;
    public bool isBurst;


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


    [Header("Scale & Positioning")]
    public Vector3 scale;
    public Vector3 position;
    public Quaternion rotation;
    public Quaternion shootRotate;


    [Header("Burst/Shotgun info")]
    public float burstTime;
    public float burstDelay;
    public float rechamberTime;
    public int burstAmount;
    public int pelletAmount;


    [Header("Audio")]
    public AudioClip[] shotSound;
    [Range(0, 1)] public float shotSoundVol;
    internal Vector3 postion;
}
