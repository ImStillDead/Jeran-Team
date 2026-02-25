using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.ProBuilder.MeshOperations;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuMain;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject settingsMenu;

    [SerializeField] GameObject VolumeSlider;
    [SerializeField] GameObject reticle;

    [SerializeField] int objectiveTimerDelay;
    [SerializeField] int maxSpawn;

    [SerializeField] TMP_Text magazine_text;
    [SerializeField] TMP_Text maxMagsize_text;
    [SerializeField] TMP_Text killCount_text;
    [SerializeField] TMP_Text Objective_timer_text;

    [SerializeField] Color activeColor = Color.white;
    [SerializeField] Color oldColor = Color.gray;


    [SerializeField] GameObject dialog_prefab;
    [SerializeField] Transform dialogParent;
    public List<TMP_Text> listofDialog = new List<TMP_Text> { }; //wip

    [SerializeField] GameObject Objective_prefab;
    [SerializeField] Transform missonParent;
    public List<TMP_Text> missions = new List<TMP_Text> {}; //wip
    int maxTextprefabs = 5;


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
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }


        instance = this;
        timeScaleOrg += Time.timeScale;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        player = GameObject.FindWithTag("Player");

        playerScript = player.GetComponent<PlayerController>();
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
            else if (menuActive == menuPause) stateUnpause();
        }

        startMission();


    }
    private void startMission()
    {
        if (startTimer || objectiveCompleted) Objective_timer_text.gameObject.SetActive(true); 

        else Objective_timer_text.gameObject.SetActive(false);
        
        if (startTimer)objectiveStartTimer();
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
        if (reticle != null) reticle.SetActive(true); 
        isPaused = false;
        Time.timeScale = timeScaleOrg;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        menuActive.SetActive(false);
        menuActive = null;

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

        magazine_text.text = magsize.ToString();
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
        if(enemyCount >= maxSpawn)
        {
            canSpawn = false;
        }else if(enemyCount < maxSpawn)
        {
            canSpawn = true;
        }
    }

    void objectiveStartTimer()
    {
        float remaintime = objectiveTimerDelay * 0.40f;

        Objective_timer_text.gameObject.SetActive(true);


        objectiveTimer -= Time.deltaTime;


        if (objectiveTimer < 0f) objectiveTimer = 0f;

        int minutes = Mathf.FloorToInt(objectiveTimer / 60);
        int seconds = Mathf.FloorToInt(objectiveTimer % 60);

        Objective_timer_text.text = string.Format("{0:00}: {1:00}", minutes, seconds);



        if (objectiveTimer <= remaintime)
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
            addMission("RUN TO THE EXIT");

        }

    }
    public void StartObjective()
    {
        if (!startTimer && !objectiveCompleted)
        {
            objectiveTimer = objectiveTimerDelay;
            startTimer = true;
            objectiveCompleted = false;
            addMission("survive");

        }
    }

    public bool IsObjectiveComplete()
    {
        return objectiveCompleted;
    }

    public void addMission(string msg)
    {

        foreach (TMP_Text oldMission in missions)
        {
            oldMission.color = oldColor;
            oldMission.fontSize = 40;
        }

        GameObject obj = Instantiate(Objective_prefab, missonParent);
        TMP_Text text = obj.GetComponent<TMP_Text>();
        text.text = msg;
        text.color = activeColor;
        text.fontSize = 60;
        obj.transform.SetAsFirstSibling();

        missions.Add(text);


        while (missions.Count > maxTextprefabs)
        {   
            TMP_Text oldest = missions[0];
            Destroy(oldest);
            missions.RemoveAt(0);
        }

    }

    public void addDialog(string msg)
    {
        foreach (TMP_Text oldDialog in listofDialog)
        {
            oldDialog.color = oldColor;
            oldDialog.fontSize = 40;
            StartCoroutine(fadeText(oldDialog, 3));
        }

        GameObject obj = Instantiate(dialog_prefab, dialogParent);
        TMP_Text text = obj.GetComponent<TMP_Text>();
        text.text = msg;
        text.color = activeColor;
        text.fontSize = 60;
        obj.transform.SetAsLastSibling();
        StartCoroutine(fadeText(text, 9));

        listofDialog.Add(text);

        while (listofDialog.Count < 1)
        {
            TMP_Text oldest = listofDialog[0];
            Destroy(oldest.gameObject);
            listofDialog.RemoveAt(0);
        }


    }

    IEnumerator fadeText(TMP_Text Text, float duration)
    {
        if (Text == null) yield break;

        float elapsed = 0f;

        Color original = Color.white;
        Color target = Color.clear;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Text.color = Color.Lerp(original, target, elapsed / duration);
            yield return null;
        }


        if(Text != null)
        Destroy(Text.gameObject);
    }

}
