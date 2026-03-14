using System;
using System.IO;
using UnityEngine;

public class FileManager 
{
    private string dataDirPath = "";
    public string dataFileName = "";

    public FileManager(string  dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("No save file found");
            return null;
        }

        string loadData = System.IO.File.ReadAllText(fullPath);
        GameData loadedData = JsonUtility.FromJson<GameData>(loadData);

        Debug.Log("File Loaded" + fullPath);
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(dataDirPath);

            string storeData = JsonUtility.ToJson(data);
            File.WriteAllText(fullPath, storeData);
            Debug.Log("Save File Complete" + fullPath );
        }
        catch (Exception e)
        {
            Debug.Log("Error Saving Data to File: " + fullPath + "\n" + e);
            Debug.Log($"[FileManager] Failed to save to: {fullPath}\n{e.Message}\n{e.StackTrace}");
        }

    }
}
