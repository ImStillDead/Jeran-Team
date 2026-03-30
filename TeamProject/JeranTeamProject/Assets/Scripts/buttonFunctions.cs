using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class ButtonFunctions : MonoBehaviour
{
    GameManager manager;
    Button btn;

    [Header("Scene Exclusion")]
    public List<string> excludedScenes = new List<string>()
    {
        "MainDevScene",
        "HubWorld",
        "DevDisplay"
    };
    private int Levelcompletecounter;

    private static ButtonFunctions instance;

    void Awake()
    {
        btn = GetComponentInChildren<Button>();

        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        manager = GameManager.instance ?? FindAnyObjectByType<GameManager>();
    }

    public void Resume()
    {
        if (manager != null && manager.menus != null)
            manager.menus.stateUnpause();
    }

    public void Save() { }
    public void LevelSelect() { }
    public void Settings() { }

    public void Respawn()
    {
        if (manager != null && manager.playerScript != null)
        {
            manager.playerScript.SpawnPlayer();
            manager.menus?.stateUnpause();
        }
    }

    public void MainMenu()
    {
        if (DataManager.instance == null)
            DataManager.instance?.SaveData(DataManager.instance.hubData);
        if (manager != null)
            manager?.UpdateRun();
        MenuController.Instance?.LoadSceneWithProgress(0);
    }

    public void ContinueRun()
    {
        if (DataManager.instance != null && DataManager.instance.currentRun != null)
        {
            GameData load = DataManager.instance.currentRun.LoadRun();
            MenuController.Instance?.LoadSceneWithProgress(load.sceneIndex);
            manager?.menus?.stateUnpause();
            manager?.LoadRun();
        }
    }

    public void ChooseRun(string fileName)
    {
        if (DataManager.instance != null)
        {
            DataManager.instance.fileName = fileName;
            DataManager.instance.currentLoad?.LoadGame();
        }
    }

    public void Restart()
    {
        if (manager?.playerScript != null)
            manager.playerScript.SpawnPlayer();
    }

    public void Quit()
    {
        if (manager != null)
        {
            try { manager.UpdateRun(); }
            catch (Exception e) { Debug.LogWarning("Failed to save: " + e.Message); }
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void NewGame()
    {
        DataManager.instance?.NewGame();
        MenuController.Instance?.LoadSceneWithProgress(1);
    }

    public void DevDisplay()
    {
        MenuController.Instance?.LoadSceneWithProgress("DevDisplay");
    }

    public void StartGame()
    {
        MenuController.Instance?.LoadSceneWithProgress(1);
    }

    public void NextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        index += 1;

        if (index >= SceneManager.sceneCountInBuildSettings)
        {
            DataManager.instance?.SaveData(DataManager.instance.hubData);
            manager?.UpdateRun();
            MenuController.Instance?.LoadSceneWithProgress(0);
        }
        else
        {
            MenuController.Instance?.LoadSceneWithProgress(index);
            if (manager != null)
            {
                manager.sceneIndex = index;
                manager.resetObjective();
                manager.UpdateRun();
                manager.playerScript?.SpawnPlayer();
                manager.playerScript?.LoadRun();
                manager.menus?.stateUnpause();
            }
        }
    }

    public void nextRandLevel()
    {
        DontDestroyOnLoad(this);
        Levelcompletecounter++;
        LoadRandomScene();
    }

    public void backToHub(bool win)
    {
        int exitpretence = Levelcompletecounter % 5;

        if (exitpretence == 0 && win == true)
        {
            activebuttonInCode(true);
            MenuController.Instance?.LoadSceneWithProgress(1);
        }
        else if (win == false)
        {
            activebuttonInCode(true);
            MenuController.Instance?.LoadSceneWithProgress(1);
        }
        else
        {
            activebuttonInCode(false);
        }
    }

    public void activebuttonInCode(bool active)
    {
        btn.interactable = active;
    }

    public void LoadRandomScene()
    {
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        List<string> validScenes = new List<string>();

        for (int i = 0; i < totalScenes; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (!excludedScenes.Contains(sceneName))
            {
                validScenes.Add(sceneName);
            }
        }

        if (validScenes.Count == 0)
        {
            Debug.LogWarning("No valid scenes to load!");
            return;
        }

        // Keep picking until we get a valid scene (though validScenes already guarantees it)
        string sceneToLoad = null;
        while (sceneToLoad == null)
        {
            int ran = UnityEngine.Random.Range(0, validScenes.Count); // MUST use validScenes.Count
            sceneToLoad = validScenes[ran];
        }

        Debug.Log("Loading scene: " + sceneToLoad);
        loadSceneAtStart(sceneToLoad);

        if (sceneToLoad != null)
        {
            manager.menus.menuWin.SetActive(false);
        }
    }

    private void loadSceneAtStart(string input)
    {
        MenuController.Instance?.LoadSceneWithProgress(input);
    }
}