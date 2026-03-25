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
    [SerializeField] private Button winMainButton;
    [SerializeField] GameObject menuLose;
    [SerializeField] private Button loseMainButton;

    public bool isPaused;
    float timeScaleOrg;



    GameManager manager;

    private void Awake()
    {
        manager = GameManager.instance; //pretty sure this has no uses.
    }


    void Start()
    {
        timeScaleOrg = Time.timeScale;

        if (menuPause != null && firstMainButton != null )
        {
            setMenuButton(menuPause);

            setFirstButton(firstMainButton);   
                
                   //this is so we dont have to click on a button within the main menu and seamlessly use the buttons
                   //only within the main menu of course. 
        }   

    }

    public void Update()
    {
        if(statePause(false)) stateUnpause();


    }

    void setFirstButton(Button button)
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button.gameObject);   //this is so we dont have to click on a button within the main menu and seamlessly use the buttons
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
        if (menuActive != null) menuActive.SetActive(false);  //when making a new button make sure the button that goes is supposed to open the submenu make sure to have this activate as well,
                                                              // it will update the current active menu which will give you the ability to use controller or keyboard imputs as controlls for the menu.
        menuActive = menu;
        menuActive.SetActive(true);

        menuButtonController(menuActive);

        cardHolder cardUI = FindAnyObjectByType<cardHolder>();
        if (cardUI != null)
        {
            cardUI.updateCards();
        }

      //  Debug.Log(menuActive + " is active");
    }

    public void openMenuButton(GameObject menu)
    {
        statePause(true);                                     
        menuActive = menu;
        menuActive.SetActive(true);

        menuButtonController(menuActive);
       // Debug.Log(menuActive + " is active");
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);

    } //this is the main function that allows us to use contoller input as controlls for menus.

}
