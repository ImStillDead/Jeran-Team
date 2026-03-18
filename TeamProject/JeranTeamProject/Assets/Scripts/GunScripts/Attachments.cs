using System;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class Attachments : ScriptableObject
{
    [Range(1, 100)] public int damageAmount;
    [Range(0.05f, 10f)] public float shootRate;
    [Range(1, 500)] public int magSizeMax;
    [Range(0.5f, 10)] public float reloadTime;
    public float spread;
    public bool isShotgun;
    public bool isBurst;
    public float burstTime;
    public float burstDelay;
    public float rechamberTime;
    public int burstAmount;
    public int pelletAmount;
}
