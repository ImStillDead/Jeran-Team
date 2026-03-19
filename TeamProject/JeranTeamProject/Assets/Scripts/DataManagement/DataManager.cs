using System.Collections.Generic;
using UnityEngine;

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
        hubData = new GameData();
        gameData = new GameData();
        SetHub(hubData);
        SaveRun(gameData);
    }
   public void SetHub(GameData data)
    {
        hubData.sceneIndex = data.sceneIndex; 
        hubData.playerData = data.playerData;
        hubData.player = data.player;
        SaveData(hubData);
    }
    public void SetRun(GameData data)
    {
        gameData.sceneIndex = data.sceneIndex;
        gameData.playerData = data.playerData;
        gameData.player = data.player;
        gameData.currentpickUps = data.currentpickUps;
        SaveRun(gameData);
    }
    
}
