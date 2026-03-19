using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSelect : ScriptableObject
{
    public GameObject mesh;
    public float HP;
    public float speed;
    public float speedMod;
    public int jumpMax;
    public float jumpSpeed;
    public float jumpChargeMax;
    public float jumpChargeRate;
    public List<GunStats> gunList;
    public List<Pickups> itemList;
   
}
