using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Loading Screen UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressSlider;

    [Header("Loading Settings")]
    [SerializeField] private float minimumLoadTime = 2f;
    [SerializeField] private float sliderSmoothingSpeed = 5f;

    private static ButtonFunctions instance;

    void Awake()
    {
        btn = GetComponentInChildren<Button>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
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
        if(DataManager.instance == null)
        DataManager.instance?.SaveData(DataManager.instance.hubData);
        if(manager != null)
        manager?.UpdateRun();
        LoadSceneWithProgress(0);
    }

    public void ContinueRun()
    {
        if (DataManager.instance != null && DataManager.instance.currentRun != null)
        {
            GameData load = DataManager.instance.currentRun.LoadRun();
            LoadSceneWithProgress(load.sceneIndex);
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
        LoadSceneWithProgress(1);
    }

    public void DevDisplay()
    {
        LoadSceneWithProgress("DevDisplay");
    }

    public void StartGame()
    {
        LoadSceneWithProgress(1);
    }

    public void NextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        index += 1;

        if (index >= SceneManager.sceneCountInBuildSettings)
        {
            DataManager.instance?.SaveData(DataManager.instance.hubData);
            manager?.UpdateRun();
            LoadSceneWithProgress(0);
        }
        else
        {
            LoadSceneWithProgress(index);
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
            LoadSceneWithProgress(1);
        }
        else if(win == false)
        {
            activebuttonInCode(true);
            SceneManager.LoadScene(1);
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

        int ran = UnityEngine.Random.Range(0, totalScenes);
        LoadSceneWithProgress(validScenes[ran]);
    }

    private void LoadSceneWithProgress(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private void LoadSceneWithProgress(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        if (progressSlider != null)
            progressSlider.value = 0;

        float startTime = Time.time;
        float currentDisplayProgress = 0f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            currentDisplayProgress = Mathf.Lerp(currentDisplayProgress, targetProgress, Time.deltaTime * sliderSmoothingSpeed);

            if (progressSlider != null)
                progressSlider.value = currentDisplayProgress;

            yield return null;
        }

        while (currentDisplayProgress < 0.99f)
        {
            currentDisplayProgress = Mathf.Lerp(currentDisplayProgress, 1f, Time.deltaTime * sliderSmoothingSpeed);

            if (progressSlider != null)
                progressSlider.value = currentDisplayProgress;

            yield return null;
        }

        if (progressSlider != null)
            progressSlider.value = 1f;

        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minimumLoadTime)
        {
            float remainingTime = minimumLoadTime - elapsedTime;
            yield return new WaitForSeconds(remainingTime);
        }

        asyncLoad.allowSceneActivation = true;
        yield return new WaitForSeconds(0.1f);

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        if (progressSlider != null)
            progressSlider.value = 0;

        float startTime = Time.time;
        float currentDisplayProgress = 0f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            currentDisplayProgress = Mathf.Lerp(currentDisplayProgress, targetProgress, Time.deltaTime * sliderSmoothingSpeed);

            if (progressSlider != null)
                progressSlider.value = currentDisplayProgress;

            yield return null;
        }

        while (currentDisplayProgress < 0.99f)
        {
            currentDisplayProgress = Mathf.Lerp(currentDisplayProgress, 1f, Time.deltaTime * sliderSmoothingSpeed);

            if (progressSlider != null)
                progressSlider.value = currentDisplayProgress;

            yield return null;
        }

        if (progressSlider != null)
            progressSlider.value = 1f;

        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minimumLoadTime)
        {
            float remainingTime = minimumLoadTime - elapsedTime;
            yield return new WaitForSeconds(remainingTime);
        }

        asyncLoad.allowSceneActivation = true;
        yield return new WaitForSeconds(0.1f);

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }
}