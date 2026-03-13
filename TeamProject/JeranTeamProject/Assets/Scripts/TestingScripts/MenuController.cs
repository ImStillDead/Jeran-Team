using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [SerializeField] GameObject menuActive;


    [SerializeField] GameObject menuPause;
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
    }

    public void pauseMenu()
    {

        if (menuActive == null)
        {
            statePause(true);
            menuActive = menuPause;
            menuActive.SetActive(true);
            manager.menuButtonController(menuActive);

        }
        else if (menuActive == menuPause)
        {
            stateUnpause();
    
        }

        manager.menuButtonController(menuActive);
    }
    public void exitSubMenu (GameObject menu)
    {
        menu.SetActive(false);
        menuActive = menuPause;
        menuPause.SetActive(true);
        manager.menuButtonController(menuActive);
        Debug.Log(menuActive + " is active");

    }
    public void setSubMenuButton(GameObject menu)
    {
        menuActive = menu;
        manager.menuButtonController(menu);
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

        menuActive.SetActive(false);
        menuActive = null;
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

}
