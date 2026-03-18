using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Unity.Properties;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour //ISave
{
    public static DataManager instance;
    public static GameManager manager;
    private GameData gameData;
    public GameData hubData;
    public string fileName;
    public FileManager currentLoad;
    public FileManager currentRun;

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
       
        if (currentLoad == null)
        {
            currentLoad = new FileManager(Application.persistentDataPath, fileName); //i addded this so it has a file to work off of if there is no save file to begin with, i think newgame should do the same                                                                                            
        }                                                                            // either which works. - cris
        if(currentRun == null)
        {
            currentRun = new FileManager(Application.persistentDataPath, "Current.json");
        }
        
    }
    
    public void SaveRun(GameData data)
    {
        currentRun.SaveRun(data);
    }
    public void SaveData(GameData data)
    {
        currentLoad.SaveGame(data);
    }
    public GameData LoadData()
    {
        return currentLoad.LoadGame();
    }
    public GameData LoadRun()
    {
        return currentRun.LoadRun();
    }
    public void NewGame()
    {
        manager.StartData();
        SaveData(hubData);
    }
   public void UpdateHub(GameData data)
    {


        hubData.HP = data.HP;
        hubData.speed = data.speed;
        hubData.sprintMod = data.sprintMod;
        hubData.jumpSpeed = data.jumpSpeed;
        hubData.jumpChargeMax = data.jumpChargeMax;
        hubData.jumpChargeRate = data.jumpChargeRate;
        hubData.jumpMax = data.jumpMax;
        List<GunStats> gunList = new List<GunStats>();
        List<Pickups> itemList = new List<Pickups>();
        SaveData(hubData);
        Debug.Log("Hub Updated" +  hubData);
    }
    
}
