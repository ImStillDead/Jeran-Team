using System;
using System.IO;
using UnityEngine;

public class FileManager 
{
    private string dataDirPath = "";
    public string dataFileName = "";
    private string currentRunFile = "Current.json";

    public FileManager(string  dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }
    public GameData LoadRun()
    {
        string fullPath = Path.Combine(dataDirPath, currentRunFile);
        string loadData = System.IO.File.ReadAllText(fullPath);
        GameData loadedData = JsonUtility.FromJson<GameData>(loadData);
        Debug.Log("File Loaded" + fullPath);
        return loadedData;
    }
    public GameData LoadGame()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        string loadData = System.IO.File.ReadAllText(fullPath);
        GameData loadedData = JsonUtility.FromJson<GameData>(loadData);
        Debug.Log("File Loaded" + fullPath);
        return loadedData;
    }
    public void SaveRun(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, currentRunFile);
        try
        {
            string storeData = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(fullPath, storeData);
            Debug.Log("Save File Complete" + fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error Saving Data to File: " + fullPath + "\n" + e);
        }

    }
    public void SaveGame(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            string storeData = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(fullPath, storeData);
            Debug.Log("Save File Complete" + fullPath );
        }
        catch (Exception e)
        {
            Debug.LogError("Error Saving Data to File: " + fullPath + "\n" + e);
        }

    }
}
