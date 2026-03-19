using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSelect : ScriptableObject
{
    public GameObject mesh;
    public float HPMax;
    public float HP;
    public float speed;
    public float speedMod;
    public int jumpMax;
    public float jumpSpeed;
    public float jumpChargeMax;
    public float jumpChargeRate;
    public List<GunStats> gunList = new List<GunStats>();
    public List<Pickups> itemList = new List<Pickups>();
   
}
