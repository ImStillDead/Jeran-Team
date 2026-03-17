using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.SubsystemsImplementation;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public MenuController menus;
    [SerializeField] List<GameObject> itemsCase;

    [SerializeField] GameObject VolumeSlider;
    [SerializeField] GameObject reticle;

    [SerializeField] int objectiveTimerDelay;
    [SerializeField] int maxSpawn;

    [SerializeField] TMP_Text magazine_text;
    [SerializeField] TMP_Text maxMagsize_text;
    [SerializeField] TMP_Text maxAmmo_text;
    [SerializeField] TMP_Text Objective_timer_text;

    [SerializeField] Color activeColor = Color.white;
    [SerializeField] Color oldColor = Color.gray;


    [SerializeField] GameObject dialog_prefab;
    [SerializeField] Transform dialogParent;
    public List<TMP_Text> listofDialog = new List<TMP_Text> { }; //wip

    [SerializeField] GameObject Objective_prefab;
    [SerializeField] Transform missonParent;
    public List<TMP_Text> missions = new List<TMP_Text> { }; //wip
    int maxTextprefabs = 5;

    public TMP_Text moneyCount;
    public Image PlayerHP_bar;
    public Image XP_bar;
    public TMP_Text levelText;
    public GameObject playerDamageFlash;
    public TMP_Text heathNum;
    public TMP_Text maxHealthNum;
    public TMP_Text killCount_text;



    public GameData currentGameData;

    public GameObject player;
    public PlayerController playerScript;
    public Light objectiveLight;
    public GameObject doorLights;
    public GameObject playerSpawn;
    public GameObject playerCheckpointPop;


    public int DAYs;
    public int sceneIndex;
    int itemIndex;
    public int enemyCount;
    public int killCount;
    public bool canSpawn;
    public bool startTimer;
    public bool objectiveCompleted;
    public float objectiveTimer;
    int magSize;
    int maxMagSize;

    public float experience;
    public float levelUpCap = 50f;
    float increaseXpCap = 1.25f;
    public float level;

    private supportGameProgression prog;

    void Awake()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex; //did alittle clean up for the awake method, had alot of sceneIndex within it. and mulitple player = gameobject---- 

        if(Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }

        instance = this;

        if (instance == null)
        {
            instance = this;
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if (player != null)
        {
            playerSpawn = GameObject.FindWithTag("PlayerSpawn");
            playerScript = player.GetComponent<PlayerController>();
        }

    }

    void Start()
    {
        menus = Object.FindAnyObjectByType<MenuController>();
        menus.stateUnpause();

        moneyCount.text = playerScript.getplayerMoney().ToString();
        prog = GetComponent<supportGameProgression>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            menus.pauseMenu();
        }

        if (prog != null)
        {
            prog.getkills(killCount);
        }

        if (player != null)
        {
            startMission();
        }

        levelUp();

        reticle.SetActive(!menus.isPaused);
    }

   
    private void startMission()
    {
        if (startTimer || objectiveCompleted) Objective_timer_text.gameObject.SetActive(true);

        else Objective_timer_text.gameObject.SetActive(false);

        if (startTimer) objectiveStartTimer();
    }

    public void loadMain()
    {
        SceneManager.LoadScene(0);
    }

    public void levelSelect(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void ammocount(int mag, int maxMag)
    {
        magSize = mag;
        maxMagSize = maxMag;

        magazine_text.text = magSize.ToString();
        maxMagsize_text.text = maxMagSize.ToString();

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
    public void resetObjective()
    {
        objectiveCompleted = false;
        startTimer = false;
        startMission();

    }
    public void enemyBoardCount(int count)
    {
        enemyCount += count;
        if (enemyCount >= maxSpawn)
        {
            canSpawn = false;
        }
        else if (enemyCount < maxSpawn)
        {
            canSpawn = true;
        }
    }

    void objectiveStartTimer()
    {
        Color green = Color.green;
        green.a = 1f;
        objectiveLight.color = green;
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
            doorLights.SetActive(true);

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
            addDialog("");
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
        int amounOfUses = 0;

        amounOfUses++;

        if (amounOfUses == 1)
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


        if (Text != null)
            Destroy(Text.gameObject);
    }
    public void updateItem(int index)
    {
        itemsCase[itemIndex].SetActive(false);
        itemIndex = index;
        itemsCase[index].SetActive(true);
    }
    
    public void saveCurrentRun()
    {
        if(currentGameData == null)
        {
            currentGameData = new GameData();
        }
        currentGameData.sceneIndex = sceneIndex;
        currentGameData.player = player;
        currentGameData.playerScript = playerScript;
        DataManager.instance.Save(currentGameData);
    }
    public void loadCurrentRun()
    {
        player = currentGameData.player;
        playerScript = currentGameData.playerScript;
    }

    public int randomNumberPicker(int amount)
    {
        int item = Random.Range(0, amount);


        return item;
    }

     public void guiAlwaysFacePlayer(GameObject obje)
    {
        if (obje == null) return;


        Vector3 playDir = player.transform.position - obje.transform.position;
        
        playDir.y = 0;

        obje.transform.rotation = Quaternion.LookRotation(-playDir);

    }

    public void giveXP(int XP)
    {
        experience += XP;

    }
    
    public void levelUp()
    {
        float tempMaxHP = playerScript.getMaxHP();


        if(experience >= levelUpCap)
        {
            Debug.Log("you have leveled up");
            level++;
            experience -= levelUpCap;
            levelUpCap *= increaseXpCap;

            playerScript.setMaxHp(playerStatUpgrade(tempMaxHP));
            playerScript.Heal((int)tempMaxHP/4);

        }


    }

    public float playerStatUpgrade(float stat)
    {
        float statMultiplier = 1.15f;

        if (level >= 20)
        {
            statMultiplier = 1.01f; 
        }
        else if (level >= 15)
        {
            statMultiplier = 1.05f;
        }
        else if (level >= 10)
        {
            statMultiplier = 1.10f; 
        }
        else
        {
            statMultiplier = 1.15f; 
        }


        stat *= statMultiplier;

        Debug.Log("increase " + stat);

        return stat;
    }
    public float enemytatUpgrade(float stat)
    {
        float statMultiplier = 1.05f; //maybe do a system where it edits how fast the zombies addabt to the player.

        stat *= statMultiplier;

        return stat;
    }



}
