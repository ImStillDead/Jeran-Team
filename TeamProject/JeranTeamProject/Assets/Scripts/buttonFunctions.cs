using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    MenuController menu;

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
        menu.stateUnpause();
    }
    public void MainMenu()
    {
        GameManager.instance.loadMain();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.menus.stateUnpause();
    }
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    public void StartGame()
    {
        GameManager.instance.levelSelect(2);
    }
    public void nextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        index += 1;
        SceneManager.LoadScene(index);
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
