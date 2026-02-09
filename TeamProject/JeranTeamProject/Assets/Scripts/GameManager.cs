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


    [SerializeField] TMP_Text magazine_text;
    [SerializeField] TMP_Text maxMagsize_text;
    public Image PlayerHP_bar;
    public GameObject playerDamageFlash;

    public GameObject player;
    public PlayerController playerScript;
    public bool isPaused;
    float timeScaleOrg;
    int magsize;
    int maxMagsize;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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

    }
    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

}
