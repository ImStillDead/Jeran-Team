using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Unity.Properties;
using UnityEngine;

public class DataManager : MonoBehaviour, ISave
{
    public static DataManager instance;
    private GameData gameData;
    public string fileName = "saveData.json";
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
        gameData = new GameData();

        if (currentLoad == null)
        {
            currentLoad = new FileManager(Application.persistentDataPath, fileName);
        }

        currentLoad.Save(gameData);
        fullPath = Path.Combine(Application.persistentDataPath, fileName);
    }
 
    public void Save(GameData data)
    {
        if (currentLoad == null)
        {
            Debug.LogWarning("FileManager not initialized. Creating a new one.");
            currentLoad = new FileManager(Application.persistentDataPath, fileName);
        }

        if (data == null)
        {
            Debug.LogWarning("GameData is null. Cannot save.");
            return;
        }

        currentLoad.Save(data);
        Debug.Log("Saved to: " + fullPath);
    }
    
    public GameData Load()
    {
        if(currentLoad == null)
        {
            currentLoad = new FileManager(Application.persistentDataPath, fileName);
        }
        gameData = currentLoad.Load();

        if(gameData == null)
        {
            NewGame();
        }


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
