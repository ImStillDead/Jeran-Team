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
        if(GameManager.instance.playerScript != null)
        {
            GameManager.instance.playerScript.updateStats();
            
        }
    }
    public void ContinueRun()
    {
        if (GameManager.instance.player != null)
        {
            SceneManager.LoadScene(DataManager.instance.currentRunStats[0]);
            GameManager.instance.playerScript.getRunStats();
            GameManager.instance.stateUnpause();
            GameManager.instance.playerScript.instance.updateGun();
        }

    }
    public void Restart()
    {
        GameManager.instance.playerScript.spawnPlayer();
        GameManager.instance.stateUnpause();
    }
    public void Quit()
    {
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
        GameManager.instance.stateUnpause();
        GameManager.instance.resetObjective();
        GameManager.instance.playerScript.instance.updateStats();
        GameManager.instance.playerScript.instance.updateGun();

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
