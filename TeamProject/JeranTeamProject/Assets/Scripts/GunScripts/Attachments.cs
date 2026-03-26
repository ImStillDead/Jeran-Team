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
    public AttachmentType attachmentType;
    public Transform position;
    public Vector3 offset;
    public GameObject attachmentModel;
    public bool isEquipped;

    [Header("Modifiers")]
    [Range(5f, 75f)] public float adsMod;
    [Range(0f, 100f)] public float spreadMod;
    [Range(0f, 100f)] public float hipSpreadMod;
    [Range(0f, 100f)] public float recoilMod;
    [Range(0f, 100f)] public float ammoCountMod;
}
