using System;
using UnityEngine;
using UnityEngine.Rendering;



public enum AttachmentType
{
    Sights,
    Magazines,
    Foregrips,
    Laser
}

[CreateAssetMenu]
public class Attachments : ScriptableObject
{
    [Header("Placement")]
    AttachmentType attachmentType;
    Vector3 position;
    GameObject attachmentModel;
    public bool isEquipped;

    [Header("Modifiers")]
    [Range(5f, 75f)] public float adsMod;
    [Range(0f, 100f)] public float spreadMod;
    [Range(0f, 100f)] public float hipSpreadMod;
    [Range(0f, 100f)] public float recoilMod;
    [Range(0f, 100f)] public float ammoCountMod;
}
