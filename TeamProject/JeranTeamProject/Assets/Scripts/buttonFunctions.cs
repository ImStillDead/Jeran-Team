using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void resume()
    {
        GameManager.instance.stateUnpause();
    }
    public void Play()
    {
        SceneManager.LoadScene("JeranDevScene");
    }
    public void LevelSelect()
    {

    }
    public void Settings()
    {
        
    }
    public void MainMenu()
    {
        
    }

    // Update is called once per frame
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
}
