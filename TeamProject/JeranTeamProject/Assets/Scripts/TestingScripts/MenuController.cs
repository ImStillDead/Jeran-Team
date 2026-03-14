using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [SerializeField] GameObject menuActive;


    public GameObject menuPause;
    [SerializeField] private Button firstMainButton;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    public bool isPaused;
    float timeScaleOrg;



    GameManager manager;

    private void Awake()
    {
        manager = GameManager.instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeScaleOrg = Time.timeScale;

        if (menuPause != null && firstMainButton != null )
        {
            setMenuButton(menuPause);
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstMainButton.gameObject);
            }
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
        Debug.Log(menuActive + " is active");
    }

    public bool statePause(bool activeRet)
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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
        statePause(true);
        menuActive = menuWin;
        menuActive.SetActive(true);
        manager.killCount_text.text = manager.killCount.ToString();
    }

    public void youLose()
    {
        statePause(true);
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void menuButtonController(GameObject menuNameHere)
    {
        Button firstButton = menuNameHere.GetComponentInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);

    } 

}
