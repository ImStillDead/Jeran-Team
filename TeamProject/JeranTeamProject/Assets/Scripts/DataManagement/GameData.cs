using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    //GameStats
    public int sceneIndex;
    public Vector3 playerPos;
    public List<GameObject> currentpickUps = new List<GameObject>();
    //playerStats
    //public float HP;
    //public float speed;
    //public float sprintMod;
    //public float jumpSpeed;
    //public float jumpChargeMax;
    //public float jumpChargeRate;
    //public int jumpMax;
    //public List<GunStats> gunList = new List<GunStats>();
    //public List<Pickups> itemList = new List<Pickups>();
    public CharacterSelect character;
}
    
