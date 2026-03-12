using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Properties;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public List<int> basePlayerStats = new List<int>();
    public List<int> currentRunStats = new List<int>();
    public List<GunStats> currentGuns = new List<GunStats>();
    public Quaternion playerPos;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
 
  
}
