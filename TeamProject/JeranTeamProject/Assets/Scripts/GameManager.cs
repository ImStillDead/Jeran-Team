using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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


    public Image PlayerHP_bar;
    public GameObject playerDamageFlash;

    public TMP_Text killCount_text;

    public GameObject player;
    public PlayerController playerScript;
    public Light objectiveLight;
    public GameObject doorLights;
    public GameObject playerSpawn;
    public GameObject playerCheckpointPop;
    public int sceneIndex;
    int itemIndex;
    public int enemyCount;
    public int killCount;
    public bool canSpawn;
    public bool startTimer;
    public bool objectiveCompleted;
    public float objectiveTimer;
    int maxAmmoSize;
    int magSize;
    int maxMagSize;



    void Awake()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }

        instance = this;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        player = GameObject.FindWithTag("Player");
        if (player != null)

            if (instance == null)
            {
                instance = this;
            }
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            playerScript = player.GetComponent<PlayerController>();
        }
        if (player != null)
        {
            playerSpawn = GameObject.FindWithTag("PlayerSpawn");
        }
    }

    void Start()
    {
        menus = Object.FindAnyObjectByType<MenuController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Debug.LogWarning("tried opening pause menu");

            menus.pauseMenu();

        }
        if (player != null)
        {
            startMission();
        }

        reticle.SetActive(!menus.isPaused);
    }

    public void menuButtonController(GameObject menuNameHere)
    {


        Button firstButton = menuNameHere.GetComponentInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);

    }  //gives menus the ability to be controlled by keyboard input, has to be called each time you call a new menu

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
    public void ammocount(int mag, int maxMag, int maxAmmo)
    {
        magSize = mag;
        maxMagSize = maxMag;
        maxAmmoSize = maxAmmo;

        magazine_text.text = magSize.ToString();
        maxMagsize_text.text = maxMagSize.ToString();
        maxAmmo_text.text = maxAmmoSize.ToString();

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

}
