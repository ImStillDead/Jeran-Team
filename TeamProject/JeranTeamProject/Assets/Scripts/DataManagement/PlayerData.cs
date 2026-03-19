using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData 
{
    public float HP;
    public float speed;
    public float speedMod;
    public int jumpMax;
    public float jumpSpeed;
    public float jumpChargeMax;
    public float jumpChargeRate;
    public List<GunStats> gunList;
    public List<Pickups> itemList;
    public PlayerData()
    {
        HP = 50;
        speed = 8;
        speedMod = 2;
        jumpMax = 1;
        jumpSpeed = 10;
        jumpChargeMax = 3;
        jumpChargeRate = 2;
        gunList = new List<GunStats>();
        itemList = new List<Pickups>();
}
}
