using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{




    public void resume()
    {
        GameManager.instance.menus.stateUnpause();
    }
    public void LevelSelect()
    {
        GameManager.instance.levelSelect(1);
    }
    public void Settings()
    {
        
    }
    public void respawn()
    {
        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.menus.stateUnpause();
    }
    public void MainMenu()
    {
        GameManager.instance.saveCurrentRun();
        GameManager.instance.loadMain();
    }
    public void ContinueRun()
    {
        if (DataManager.instance.currentLoad != null)
        {
            GameData load = DataManager.instance.currentLoad.Load();
            SceneManager.LoadScene(load.sceneIndex);
            GameManager.instance.menus.stateUnpause();
        }
    }
    public void ChooseRun(string fileName)
    {
        DataManager.instance.fileName = fileName;
        DataManager.instance.currentLoad.Load();
    }
    public void Restart()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager.instance is null! Cannot restart.");
            return;
        }
        if (GameManager.instance.playerScript != null)
        {
            GameManager.instance.playerScript.spawnPlayer();
        }
        if (GameManager.instance.menus != null)
        {
            GameManager.instance.menus.stateUnpause();
        }
    }
    public void Quit()
    {
        //Save();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    public void NewGame()
    {

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
        GameManager.instance.sceneIndex = index;
        GameManager.instance.resetObjective();
        GameManager.instance.playerScript.updateGun();
        GameManager.instance.saveCurrentRun();
        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.menus.stateUnpause();
    }
    public void levelOne()
    {
        GameManager.instance.levelSelect(2);
    }
    public void levelTwo()
    {
        GameManager.instance.levelSelect(3);
    }
    public void levelThree()
    {
        GameManager.instance.levelSelect(4);
    }
    public void levelFour()
    {
        GameManager.instance.levelSelect(5);
    }


    
}
