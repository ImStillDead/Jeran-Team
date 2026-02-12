using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class buttonFunctions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void resume()
    {
        GameManager.instance.stateUnpause();
    }
    public void Play()
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

    public void StartGame()
    {

        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(sceneIndex);
        GameManager.instance.stateUnpause();

    }

    public void SelectLevel(int input)
    {
        if(input != 0)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex + input;
            SceneManager.LoadScene(sceneIndex);
            GameManager.instance.stateUnpause();
        }


    }




}
