using System;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    GameManager manager;


    public void Start()
    {
        manager = GameManager.instance.GetComponent<GameManager>();
    }
    public void resume()
    {
        DataManager.manager.menus.stateUnpause();
    }
    public void LevelSelect()
    {
        DataManager.manager.levelSelect(1);
    }
    public void Settings()
    {
        
    }
    public void respawn()
    {
        DataManager.manager.playerScript.spawnPlayer();
        DataManager.manager.menus.stateUnpause();
    }
    public void MainMenu()
    {
        DataManager.instance.SaveData(DataManager.instance.hubData);
        DataManager.manager.UpdateRun();
        DataManager.manager.loadMain();
    }
    public void ContinueRun()
    {
        if (DataManager.instance.currentRun != null)
        {
            GameData load = DataManager.instance.currentRun.LoadRun();
            SceneManager.LoadScene(load.sceneIndex);
            DataManager.manager.menus.stateUnpause();
            DataManager.manager.LoadRun();

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
            Debug.LogError("GameManager.instance is null! Cannot restart.");
            return;
        }
        if (DataManager.manager.playerScript != null)
        {
            DataManager.manager.playerScript.spawnPlayer();
        }
        if (DataManager.manager.menus != null)
        {
            DataManager.manager.menus.stateUnpause();
        }
    }
    public void Quit()
    {
        if (DataManager.instance != null)
        {
            // Add this method to DataManager if it doesn't exist yet
            manager.UpdateRun();
        }
        else
        {
            Debug.LogWarning("DataManager.instance is null! Skipping UpdateRun.");
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
    public void NewGame()
    {
        SceneManager.LoadScene(2);
        DataManager.instance.NewGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }
    public void nextLevel()
    {

        int index = SceneManager.GetActiveScene().buildIndex;
        index += 1;
        SceneManager.LoadScene(index);
        DataManager.manager.sceneIndex = index;
        DataManager.manager.resetObjective();
        DataManager.manager.UpdateRun();
        DataManager.manager.playerScript.spawnPlayer();
        DataManager.manager.menus.stateUnpause();
    }
    


    
}
