using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    GameManager manager;


    public void Start()
    {
        manager = GameManager.instance ?? FindAnyObjectByType<GameManager>();
    }
    public void Resume()
    {
        manager.menus.stateUnpause();
    }
    public void Save()
    {

    }
    public void LevelSelect()
    {
    }
    public void Settings()
    {
        
    }
    public void Respawn()
    {
        manager.playerScript.SpawnPlayer();
        manager.menus.stateUnpause();
    }
    public void MainMenu()
    {
        DataManager.instance.SaveData(DataManager.instance.hubData);
        manager.UpdateRun();
        SceneManager.LoadScene(0);
    }
    public void ContinueRun()
    {
        if (DataManager.instance.currentRun != null)
        {
            GameData load = DataManager.instance.currentRun.LoadRun();
            SceneManager.LoadScene(load.sceneIndex);
            manager.menus.stateUnpause();
            manager.LoadRun();

        }
    }
    public void ChooseRun(string fileName)
    {
        DataManager.instance.fileName = fileName;
        DataManager.instance.currentLoad.LoadGame();
    }
    public void Restart()
    {
        if (GameManager.instance == null)
        {
            //Debug.LogError("GameManager.instance is null! Cannot restart.");
            return;
        }
        if (manager.playerScript != null)
        {
            manager.playerScript.SpawnPlayer();
        }
        
    }
    public void Quit()
    {
        if (manager != null)
        {
            try
            {
                manager.UpdateRun();
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to save on quit: " + e.Message);
            }
        }

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    public void NewGame()
    {
        DataManager.instance.NewGame();
        SceneManager.LoadScene(2);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }
    public void NextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        index += 1;
        if (index >= SceneManager.sceneCountInBuildSettings)
        {
            DataManager.instance.SaveData(DataManager.instance.hubData);
            manager.UpdateRun();
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(index);
            manager.sceneIndex = index;
            manager.resetObjective();
            manager.UpdateRun();
            manager.playerScript.SpawnPlayer();
            manager.playerScript.LoadRun();
            manager.menus.stateUnpause();
        }
    }
    


    
}
