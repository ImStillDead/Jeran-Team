using System;
using UnityEngine;



public enum AttachmentType
{
    Sights,
    Magazines,
    Foregrips,
    Barrel
}

[CreateAssetMenu]
[Serializable]
public class Attachments : ScriptableObject
{
    [Range(1, 100)] public int damageAmountMod;
    [Range(0.05f, 10f)] public float shootRateMod;
    [Range(1, 500)] public int magSizeMaxMod;
    [Range(0.5f, 10)] public float reloadTimeMod;
    public float spreadMod;
    public bool isShotgun;
    public bool isBurst;
    public float burstTimeMod;
    public float burstDelayMod;
    public float rechamberTimeMod;
    public int burstAmountMod;
    public int pelletAmountMod;

    public Vector3 position;
    public AttachmentType attachmentType;
}
