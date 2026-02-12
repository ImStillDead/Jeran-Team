using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject reticle;
    [SerializeField] int objectiveTimerDelay;
    [SerializeField] TMP_Text magazine_text;
    [SerializeField] TMP_Text maxMagsize_text;
    public Image PlayerHP_bar;
    public GameObject playerDamageFlash;

    public GameObject player;
    public PlayerController playerScript;
    public bool isPaused;
    public bool startTimer;
    float timeScaleOrg;
    public float objectiveTimer;
    int magsize;
    int maxMagsize;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrg = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
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
        if (startTimer)
        {
            objectiveTimer += Time.deltaTime;
            if(objectiveTimer >= objectiveTimerDelay)
            {
                startTimer = false;
            }
        }

    }
    public void statePause()
    {
        reticle.SetActive(false); 
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
    }
    public void stateUnpause()
    {
        reticle.SetActive(true);
        isPaused = false;
        Time.timeScale = timeScaleOrg;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }
    public void update_ammoCount(int ammo)
    {
        maxMagsize += ammo;
        maxMagsize_text.text = maxMagsize_text.ToString();
    }

    public void youWin()
    {
        // You Win!
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
        
    }
    public void youLose()
    {
         // You Lose!
         statePause();
         menuActive = menuLose;
         menuActive.SetActive(true);

    }

    public void ammocount(int mag, int max_mag)
    {
        magsize = mag;
        maxMagsize = max_mag;

        magazine_text.text = magsize.ToString();
        maxMagsize_text.text = maxMagsize.ToString(); 
    }
    public bool objectiveCheck()
    {
        if(objectiveTimer >= objectiveTimerDelay)
        {
            return true;
        }
        else if (objectiveTimer == 0)
        {
            startTimer = true;
        }
        return false;
    }

}
