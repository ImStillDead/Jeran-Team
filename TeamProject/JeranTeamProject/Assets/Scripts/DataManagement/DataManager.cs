using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Unity.Properties;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private GameData gameData;
    public string fileName;
    private string fullPath;
    public FileManager currentLoad;
   
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void NewGame()
    {
        GameData fresh = new GameData();
        FileManager newFile = new FileManager(Application.persistentDataPath, fileName);
        currentLoad = newFile;
        gameData = fresh;
        
    }
    public void chooseFile(string fileName)
    {
        currentLoad = new FileManager(Application.persistentDataPath, fileName);
        Load(currentLoad.Load());
    }
    public void Load(GameData data)
    {
        if(fileName != currentLoad.dataFileName)
        {
            NewGame();
        }
        else
        {
            gameData = data;
            GameManager.instance.currentGameData = gameData;
            GameManager.instance.loadCurrentRun();
        }
    }
    public void Save(GameData data)
    {
        if(currentLoad == null)
        {
            NewGame();
        }
        gameData.sceneIndex = data.sceneIndex;
        gameData.player = data.player;
        gameData.playerScript = data.playerScript;
        currentLoad.Save(gameData);
    }
}
