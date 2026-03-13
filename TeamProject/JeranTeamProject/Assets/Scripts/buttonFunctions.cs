using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.instance.stateUnpause();
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
        GameManager.instance.stateUnpause();
    }
    public void MainMenu()
    {
        GameManager.instance.loadMain();
        Save();
    }
    public void ContinueRun()
    {
        GameData load = DataManager.instance.Load();
        SceneManager.LoadScene(load.sceneIndex);
    }
    public void ChooseRun(string fileName)
    {
        DataManager.instance.fileName = fileName;
        DataManager.instance.Load();
    }
    public void Restart()
    {
        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.stateUnpause();
    }
    public void Quit()
    {
        Save();
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
        GameManager.instance.playerScript.instance.updateGun();
        Save();
        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.stateUnpause();
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
    void Save()
    {
        GameData current = DataManager.instance.Load();
        current.gameManager = GameManager.instance;
        current.player = GameManager.instance.player;
        current.playerScript = GameManager.instance.playerScript;
        current.gunData = Shooting.instance;
        current.sceneIndex = current.gameManager.sceneIndex;
        current.gameManager.stateUnpause();
        DataManager.instance.Save(current);
    }
}
