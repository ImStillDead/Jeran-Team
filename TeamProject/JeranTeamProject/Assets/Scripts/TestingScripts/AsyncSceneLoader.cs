using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    [Header("Loading Screen UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text progressText;

    [Header("Settings")]
    [SerializeField] private float minimumLoadTime = 1f;

    private static AsyncSceneLoader instance;

    void Awake()
    {
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

        // Hide loading screen on start
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    public static void LoadScene(string sceneName)
    {
        if (instance != null)
            instance.StartCoroutine(instance.LoadSceneAsync(sceneName));
        else
            Debug.LogError("AsyncSceneLoader not found in scene!");
    }

    public static void LoadScene(int sceneIndex)
    {
        if (instance != null)
            instance.StartCoroutine(instance.LoadSceneAsync(sceneIndex));
        else
            Debug.LogError("AsyncSceneLoader not found in scene!");
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Show loading screen
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        if (progressSlider != null)
            progressSlider.value = 0;

        if (progressText != null)
            progressText.text = "Loading... 0%";

        float startTime = Time.time;

        // Start async load
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Wait until loading is almost done
        while (asyncLoad.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressSlider != null)
                progressSlider.value = progress;

            if (progressText != null)
                progressText.text = $"Loading... {Mathf.Round(progress * 100)}%";

            yield return null;
        }

        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minimumLoadTime)
        {
            float remainingTime = minimumLoadTime - elapsedTime;
          
            if (progressSlider != null)
                progressSlider.value = 1f;

            if (progressText != null)
                progressText.text = "Loading... 100%";

            yield return new WaitForSeconds(remainingTime);
        }

        // Activate the scene
        asyncLoad.allowSceneActivation = true;

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        if (progressSlider != null)
            progressSlider.value = 0;

        float startTime = Time.time;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressSlider != null)
                progressSlider.value = progress;

            yield return null;
        }

        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minimumLoadTime)
        {
            yield return new WaitForSeconds(minimumLoadTime - elapsedTime);
        }

        asyncLoad.allowSceneActivation = true;

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }
}