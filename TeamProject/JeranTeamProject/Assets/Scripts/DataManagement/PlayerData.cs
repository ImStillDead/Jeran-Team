using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

[Serializable]
public class PlayerData 
{
    public int level;
    public float experience;
    public float levelUpCap;
    public float HP;
    public float HPMax;
    public float speed;
    public float speedMod;
    public int jumpMax;
    public float jumpSpeed;
    public float jumpChargeMax;
    public float jumpChargeRate;
    public int dmgAmmount;
    public int money;
    public List<GunStats> gunList;
    public List<Pickups> itemList;
    public PlayerData()
    {
        level = 1;
        HP = 50;
        experience = 0;
        levelUpCap = 50;
        HPMax = 50;
        speed = 8;
        speedMod = 2;
        jumpMax = 1;
        jumpSpeed = 10;
        jumpChargeMax = 3;
        jumpChargeRate = 2;
        dmgAmmount = 3;
        money = 0;
        gunList = new List<GunStats>();
        itemList = new List<Pickups>();
}
    public void GunListSwap(List<GunStats> listSwap)
    {
        gunList.Clear();
        foreach (GunStats gun in listSwap)
        {
            gunList.Add(gun);
        }
    }
}
