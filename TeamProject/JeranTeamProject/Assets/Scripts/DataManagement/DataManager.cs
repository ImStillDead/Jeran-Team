using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Unity.Properties;
using UnityEngine;

public class DataManager : MonoBehaviour, ISave
{
    public static DataManager instance;
    private GameData gameData;
    public string fileName;
    private string fullPath;
    FileManager currentLoad;
   
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
        Save(fresh);
    }
 
    public void Save(GameData data)
    {
        currentLoad.Save(data);
    }
    
    public GameData Load()
    {
        if(currentLoad != new FileManager(Application.persistentDataPath, fileName))
        {
            NewGame();
        }
        gameData = currentLoad.Load();
        return gameData;
    }

    void ISave.Load()
    {
        if (currentLoad.dataFileName != fileName)
        {
            fullPath = Path.Combine(Application.persistentDataPath, fileName);
            currentLoad = new FileManager(Application.persistentDataPath, fileName);
            currentLoad.Load();
        }
    }
}
