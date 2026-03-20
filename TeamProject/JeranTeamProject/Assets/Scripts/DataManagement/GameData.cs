using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    //GameStats
    public int sceneIndex;
    public GameObject player;
    public List<GameObject> currentpickUps = new List<GameObject>();
    //playerStats
    public PlayerData playerData;
    //achievements here
}
    
