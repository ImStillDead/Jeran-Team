using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour, IPointerEnterHandler
{

    [SerializeField] GameObject menuActive;


    public GameObject menuPause;
    [SerializeField] private Button firstMainButton;
    [SerializeField] public GameObject menuWin;
    [SerializeField] private Button winMainButton;
    [SerializeField] public GameObject menuLose;
    [SerializeField] private Button loseMainButton;

    public bool isPaused;
    float timeScaleOrg;
    cardHolder cardUI;

    [Header("Loading Screen UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressSlider;

    [Header("Loading Settings")]
    [SerializeField] private float minimumLoadTime = 2f;
    [SerializeField] private float sliderSmoothingSpeed = 5f;

    GameManager manager;

    private static MenuController instance;
    public static MenuController Instance => instance;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        manager = GameManager.instance;

        // Initialize loading screen
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Button btn = GetComponentInParent<Button>();
        setFirstButton(btn);
    }

    void Start()
    {
        timeScaleOrg = Time.timeScale;

        if (menuPause != null && firstMainButton != null)
        {
            setMenuButton(menuPause);
            setFirstButton(firstMainButton);
        }

        if (menuWin == null && menuLose == null && winMainButton == null && loseMainButton == null)
        {
            menuWin = null;
            menuLose = null;
            winMainButton = null;
            loseMainButton = null;
            return;
        }
    }

    public void Update()
    {
        if (statePause(false)) stateUnpause();
    }

    void setFirstButton(Button button)
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }

    public void pauseMenu()
    {
        if (isPaused)
        {
            stateUnpause();
        }
        else
        {
            statePause(true);
            setMenuButton(menuPause);
        }
    }

    public void setMenuButton(GameObject menu)
    {
        if (menuActive != null) menuActive.SetActive(false);
        menuActive = menu;
        menuActive.SetActive(true);

        menuButtonController(menuActive);

        cardUI = FindAnyObjectByType<cardHolder>();
        if (cardUI != null)
        {
            cardUI.updateCards();
        }
    }

    public void openMenuButton(GameObject menu)
    {
        statePause(true);
        menuActive = menu;
        menuActive.SetActive(true);

        menuButtonController(menuActive);
    }

    public bool statePause(bool activeRet)
    {
        if (activeRet == true)
        {
            isPaused = true;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        return activeRet;
    }

    public void stateUnpause()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrg;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    }

    public void youWin()
    {
        setMenuButton(menuWin);
        setFirstButton(winMainButton);
        statePause(true);
        menuActive = menuWin;
        menuActive.SetActive(true);
        if (manager != null)
            manager.killCount_text.text = manager.killCount.ToString();
    }

    public void youLose()
    {
        setMenuButton(menuLose);
        setFirstButton(loseMainButton);
        statePause(true);
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void menuButtonController(GameObject menuNameHere)
    {
        Button firstButton = menuNameHere.GetComponentInChildren<Button>();
        if (EventSystem.current != null && firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        }
    }

    public void LoadSceneWithProgress(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    public void LoadSceneWithProgress(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // Pause the game before showing loading screen
        statePause(true);

        // Show loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            // Make sure the loading screen is on top of everything
            Canvas loadingCanvas = loadingScreen.GetComponentInParent<Canvas>();
            if (loadingCanvas != null)
            {
                loadingCanvas.sortingOrder = 1000;
            }
        }

        if (progressSlider != null)
        {
            progressSlider.value = 0;
        }

        // Force time scale to 1 for loading screen animations (but game is still "paused" via UI)
        Time.timeScale = 1f;

        float startTime = Time.realtimeSinceStartup;
        float currentDisplayProgress = 0f;

        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        if (asyncLoad == null)
        {
            if (loadingScreen != null)
                loadingScreen.SetActive(false);
            stateUnpause();
            yield break;
        }

        asyncLoad.allowSceneActivation = false;

        // Wait for loading to progress
        while (asyncLoad.progress < 0.9f)
        {
            float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            currentDisplayProgress = Mathf.Lerp(currentDisplayProgress, targetProgress, Time.deltaTime * sliderSmoothingSpeed);

            if (progressSlider != null)
                progressSlider.value = currentDisplayProgress;

            yield return null;
        }

        // Smoothly fill the rest of the bar
        while (currentDisplayProgress < 0.99f)
        {
            currentDisplayProgress = Mathf.Lerp(currentDisplayProgress, 1f, Time.deltaTime * sliderSmoothingSpeed);

            if (progressSlider != null)
                progressSlider.value = currentDisplayProgress;

            yield return null;
        }

        if (progressSlider != null)
            progressSlider.value = 1f;

        // Ensure minimum load time
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        if (elapsedTime < minimumLoadTime)
        {
            float remainingTime = minimumLoadTime - elapsedTime;
            yield return new WaitForSecondsRealtime(remainingTime);
        }

        // Allow scene activation
        asyncLoad.allowSceneActivation = true;

        // Wait for scene to actually load
        yield return new WaitForSecondsRealtime(0.1f);

        // Wait one more frame to ensure scene is ready
        yield return null;

        // Hide loading screen
        if (loadingScreen != null)
            loadingScreen.SetActive(false);

        // Unpause the game after scene is loaded
        stateUnpause();
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Pause the game before showing loading screen
        statePause(true);

        // Show loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            // Make sure the loading screen is on top of everything
            Canvas loadingCanvas = loadingScreen.GetComponentInParent<Canvas>();
            if (loadingCanvas != null)
            {
                loadingCanvas.sortingOrder = 1000;
            }
        }

        if (progressSlider != null)
        {
            progressSlider.value = 0;
        }

        // Force time scale to 1 for loading screen animations (but game is still "paused" via UI)
        Time.timeScale = 1f;

        float startTime = Time.realtimeSinceStartup;
        float currentDisplayProgress = 0f;

        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        if (asyncLoad == null)
        {
            if (loadingScreen != null)
                loadingScreen.SetActive(false);
            stateUnpause();
            yield break;
        }

        asyncLoad.allowSceneActivation = false;

        // Wait for loading to progress
        while (asyncLoad.progress < 0.9f)
        {
            float targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            currentDisplayProgress = Mathf.Lerp(currentDisplayProgress, targetProgress, Time.deltaTime * sliderSmoothingSpeed);

            if (progressSlider != null)
                progressSlider.value = currentDisplayProgress;

            yield return null;
        }

        // Smoothly fill the rest of the bar
        while (currentDisplayProgress < 0.99f)
        {
            currentDisplayProgress = Mathf.Lerp(currentDisplayProgress, 1f, Time.deltaTime * sliderSmoothingSpeed);

            if (progressSlider != null)
                progressSlider.value = currentDisplayProgress;

            yield return null;
        }

        if (progressSlider != null)
            progressSlider.value = 1f;

        // Ensure minimum load time
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        if (elapsedTime < minimumLoadTime)
        {
            float remainingTime = minimumLoadTime - elapsedTime;
            yield return new WaitForSecondsRealtime(remainingTime);
        }

        // Allow scene activation
        asyncLoad.allowSceneActivation = true;

        // Wait for scene to actually load
        yield return new WaitForSecondsRealtime(0.1f);

        // Wait one more frame to ensure scene is ready
        yield return null;

        // Hide loading screen
        if (loadingScreen != null)
            loadingScreen.SetActive(false);

        // Unpause the game after scene is loaded
        stateUnpause();
    }
}