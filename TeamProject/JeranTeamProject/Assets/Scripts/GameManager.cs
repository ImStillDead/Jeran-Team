using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuMain;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject settingsMenu;

    [SerializeField] GameObject reticle;
    [SerializeField] int objectiveTimerDelay;
    [SerializeField] TMP_Text magazine_text;
    [SerializeField] TMP_Text maxMagsize_text;
    [SerializeField] TMP_Text killCount_text;
    [SerializeField] TMP_Text Objective_timer_text;
    [SerializeField] TMP_Text Objective_text;
    public Image PlayerHP_bar;
    public GameObject playerDamageFlash;

    public GameObject player;
    public PlayerController playerScript;
    int sceneIndex;
    public int enemyCount;
    public int killCount;
    public bool canSpawn;
    public bool isPaused;
    public bool startTimer;
    public bool objectiveCompleted;
    float timeScaleOrg;
    public float objectiveTimer;
    int magsize;
    int maxMagsize;
    
    void Awake()
    {
        instance = this;
        timeScaleOrg = Time.timeScale;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        canSpawn = true;

    }



    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }

        if (startTimer || objectiveCompleted) 
        { 
            Objective_timer_text.gameObject.SetActive(true); 
            Objective_text.gameObject.SetActive(true);
        }
        else
        {
            Objective_timer_text.gameObject.SetActive(false);
            Objective_text.gameObject.SetActive(false);
        }



        if (startTimer)
        {

            objectiveStartTimer();
        }

    }


    public void statePause()
    {
        if(reticle != null) reticle.SetActive(false); //
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
    }
    public void stateUnpause()
    {
        if (reticle != null) reticle.SetActive(true); //
        isPaused = false;
        Time.timeScale = timeScaleOrg;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        menuActive.SetActive(false);
        menuActive = null;

        menuActive = null; //
    }
    public void youWin()
    {
        statePause();
        menuActive = menuWin;
        killCount_text.text = killCount.ToString();
        menuActive.SetActive(true);
    }
    public void youLose()
    {
         statePause();
         menuActive = menuLose;
         menuActive.SetActive(true);
    }
    public void OpenSettings()
    {
        statePause();

        if (menuActive != null)
            menuActive.SetActive(false);

        menuActive = settingsMenu;
        menuActive.SetActive(true);
    }
    public void CloseSettings()
    {
        if (settingsMenu != null) settingsMenu.SetActive(false);

        menuActive = menuPause;
        if (menuPause != null) menuPause.SetActive(true);
    }

    public void loadMain()
    {
        SceneManager.LoadScene(0);
    }
    public void loadNextScene()
    {
        objectiveTimer = 0;
        sceneIndex += 1;
        if (sceneIndex > SceneManager.sceneCount)
        {
            loadMain();
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
    public void levelSelect(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void ammocount(int mag, int max_mag)
    {
        magsize = mag;
        maxMagsize = max_mag;

        magazine_text.text = magsize.ToString(); // its getting a error here in unity
        maxMagsize_text.text = maxMagsize.ToString(); 
    }
    public bool objectiveCheck()
    {
        if (!startTimer && objectiveTimer <= 0f)
        {
            objectiveTimer = objectiveTimerDelay;
            startTimer = true;
        }

        return objectiveTimer <= 0f;
    }
    public void enemyBoardCount(int count)
    {   
        enemyCount += count;
        if(enemyCount >= 20)
        {
            canSpawn = false;
        }else if(enemyCount <= 20)
        {
            canSpawn = true;
        }
    }

    void objectiveStartTimer()
    {


        Objective_timer_text.gameObject.SetActive(true);
        Objective_text.gameObject.SetActive(true);

        objectiveTimer -= Time.deltaTime;


        if (objectiveTimer < 0f) objectiveTimer = 0f;

        int minutes = Mathf.FloorToInt(objectiveTimer / 60);
        int seconds = Mathf.FloorToInt(objectiveTimer % 60);

        Objective_timer_text.text = string.Format("{0:00}: {1:00}", minutes, seconds);
        Objective_text.text = "survive";

        if (objectiveTimer <= 5f)
        {

            float alpha = Mathf.Abs(Mathf.Sin(Time.time * 8f));
            Color c = Color.red;
            c.a = alpha;
            Objective_timer_text.color = c;


        }
        else
        {
            Color c = Color.white;
            c.a = 1f;
            Objective_timer_text.color = c;

        }



        if (objectiveTimer <= 0f)
        {
            startTimer = false;
            objectiveCompleted = true;

            Objective_timer_text.color = Color.white;
            Objective_text.text = "get to the exit!";

        }

    }
    public void StartObjective()
    {
        if (!startTimer && !objectiveCompleted)
        {
            objectiveTimer = objectiveTimerDelay;
            startTimer = true;
            objectiveCompleted = false;
        }
    }

    public bool IsObjectiveComplete()
    {
        return objectiveCompleted;
    }


}
