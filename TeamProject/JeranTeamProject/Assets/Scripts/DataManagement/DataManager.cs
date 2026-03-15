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
    public FileManager currentLoad;
   
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        if (currentLoad == null)
        {
            fullPath = Path.Combine(Application.persistentDataPath, fileName);
            currentLoad = new FileManager(Application.persistentDataPath, fileName); //i addded this so it has a file to work off of if there is no save file to begin with, i think newgame should do the same                                                                                            
        }                                                                            // either which works. - cris

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
        if (currentLoad == null)
        {
            Load();
            Debug.Log("No save detected, new save has been made" + Application.persistentDataPath); //this one was just to make a save if there wasnt a save yet automaticlly, i also removed
        }                                                                                           //"ref" from your parameter, since it wasnt being utilized and it was giving me issues for some reason 
                                                                                                    // can be easily added back of course - cris
        currentLoad.Save(data);
    }
    
    public GameData Load()
    {
        if(currentLoad == null)
        {
            NewGame();
        }
        gameData = currentLoad.Load();
        return gameData;
    }

    void ISave.Load(GameData data) //only added the parameter for the isave function didnt remove it cause thought you were going to do something with it. - cris
    {
        if (currentLoad.dataFileName != fileName)
        {
            fullPath = Path.Combine(Application.persistentDataPath, fileName);
            currentLoad = new FileManager(Application.persistentDataPath, fileName);
            currentLoad.Load();
        }
    }

}
