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
        GameManager.instance.OpenSettings();
    }
    public void MainMenu()
    {
        GameManager.instance.loadMain();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    public void StartGame()
    {
        GameManager.instance.levelSelect(2);
    }
    public void nextLevel()
    {
        GameManager.instance.loadNextScene();
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
