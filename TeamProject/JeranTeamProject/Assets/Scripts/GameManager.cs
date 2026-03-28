using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public enum EnemyType
{
    RegularZombie,
    HeavyZombie,
    Spitter,
    DogZombie,
    Boss1,
    Boss2,
    Boss3
}
public class GameManager : MonoBehaviour
{
    [Header("Controllers")]
    public static GameManager instance;
    public MenuController menus;
    public GameObject player;
    public PlayerController playerScript;

    [Header("Static Objects")]
    [SerializeField] CharacterSelect baseCharacter;
    [SerializeField] GameObject VolumeSlider;
    [SerializeField] public GameObject UI;
    [SerializeField] int maxSpawn;
    [SerializeField] List<GameObject> itemsCase;

    [Header("Weapon Handle")]
    [SerializeField] GameObject reticle;
    [SerializeField] TMP_Text magazine_text;
    [SerializeField] TMP_Text maxMagsize_text;
    [SerializeField] TMP_Text maxAmmo_text;
    [SerializeField] Color activeColor = Color.white;
    [SerializeField] Color oldColor = Color.gray;
    
    [Header("High Score")]
    public float gameTime;
    public int currentScore;
    public bool isGameActive = true;
    //public HighscoreTable highscoreTable;
    public int regularZombiePoints = 100;
    public int heavyZombiePoints = 250;
    public int spitterPoints = 150;
    public int dogZombiePoints = 200;
    public int boss1Points = 1000;
    public int boss2Points = 1500;
    public int boss3Points = 2000;

    [Header("Npc/Missons")]
    [SerializeField] int objectiveTimerDelay;
    [SerializeField] TMP_Text Objective_timer_text;
    [SerializeField] GameObject dialog_prefab;
    [SerializeField] Transform dialogParent;
    public List<TMP_Text> listofDialog = new List<TMP_Text> { }; //wip
    [SerializeField] GameObject Objective_prefab;
    [SerializeField] Transform missonParent;
    public List<GameObject> pickUpObjects = new List<GameObject>();
    public List<TMP_Text> missions = new List<TMP_Text> { }; //wip
    int maxTextprefabs = 5;

    [Header("Armor Handle")]
    public GameObject armorParent;
    public Image armorPrefab;
    private Image tempArmor;
    private List<Image> armors = new List<Image>();

    [Header("Image/Text INPUTS")]
    public TMP_Text moneyCount;
    public Image PlayerHP_bar;
    public Image XP_bar;
    public TMP_Text levelText;
    public GameObject playerDamageFlash;
    public TMP_Text heathNum;
    public TMP_Text maxHealthNum;
    public TMP_Text killCount_text;
    public Image reloadBar;

    [Header("Game Management")]
    public GameData gameData;
    public GameData hubData;
    public Light objectiveLight;
    public GameObject doorLights;
    public GameObject playerSpawn;
    public GameObject playerCheckpointPop;
    public int itemIndex;
    float increaseXpCap = 1.25f;
    public int DAYs;
    public int sceneIndex;
    public int enemyCount;
    public int killCount;
    public bool canSpawn;
    public bool startTimer;
    public bool objectiveCompleted;
    public float objectiveTimer;
    public float experience;
    public float levelUpCap = 50f;
    public float level;
    public supportGameProgression prog;
    //Awake Start Update
    void Awake()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex; //did alittle clean up for the awake method, had alot of sceneIndex within it. and mulitple player = gameobject---- 
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }
        if (instance == null)
        {
            instance = this;
        }
        if (instance.player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if (instance.player != null)
        {
            playerScript = player.GetComponent<PlayerController>();
            playerSpawn = GameObject.FindWithTag("PlayerSpawn");
        }
    }
    void Start()
    {
        killCount = 0;
        menus = Object.FindAnyObjectByType<MenuController>();
        menus.stateUnpause();
        prog = GetComponent<supportGameProgression>();
        if (moneyCount != null && playerScript != null)
            moneyCount.text = playerScript.GetplayerMoney().ToString();
        if(DataManager.instance != null)
        {
            StartData();
        }
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            menus.pauseMenu();
        }

        if (player != null)
        {
            startMission();
        }
        levelUp();
        reticle.SetActive(!menus.isPaused);
        if (isGameActive && !menus.isPaused && player != null)
        {
            gameTime += Time.deltaTime;
        }
    }
    public int randomNumberPicker(int amount)
    {
        int item = Random.Range(0, amount);


        return item;
    }
    //Missions and Dialaog
    private void startMission()
    {
        if (startTimer || objectiveCompleted) Objective_timer_text.gameObject.SetActive(true);

        else Objective_timer_text.gameObject.SetActive(false);

        if (startTimer) objectiveStartTimer();
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
    public void FlashText(TMP_Text text, Color flashColor, float duration)
    {
        StartCoroutine(FlashTextCoroutine(text, flashColor, duration));
    }
    private IEnumerator FlashTextCoroutine(TMP_Text text, Color flashColor, float duration)
    {
        if (text == null) yield break;

        float elapsed = 0f;

        Color original = Color.white;
        Color target = flashColor;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text.color = Color.Lerp(original, target, elapsed / duration);
            yield return null;
        }

        text.color = original;
    }
    //UI Updates
    public void Ammocount(int mag, int maxMag)
    {
        magazine_text.text = mag.ToString();
        maxMagsize_text.text = maxMag.ToString();
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
    public void updateItem(int index)
    {
        itemsCase[itemIndex].SetActive(false);
        itemIndex = index;
        itemsCase[index].SetActive(true);
    }
    public void guiAlwaysFacePlayer(GameObject obje)
    {
        if (obje == null) return;


        Vector3 playDir = player.transform.position - obje.transform.position;

        playDir.y = 0;

        obje.transform.rotation = Quaternion.LookRotation(playDir);

    }

    public void guiAlwaysFacePlayerOnPivot(GameObject obje, Transform pivot)
    {
        if (obje == null) return;


        Vector3 playDir = player.transform.position - pivot.position;

        playDir.y = 0;


        playDir.Normalize();

        float radius = Vector3.Distance(obje.transform.position, pivot.position);

        obje.transform.position = pivot.position + playDir * radius;

        obje.transform.rotation = Quaternion.LookRotation(playDir);

    }

    public void UpdateUI(PlayerData update)
    {
        moneyCount.text = update.money.ToString();
        levelText.text = update.level.ToString();
        heathNum.text = update.HP.ToString();
        maxHealthNum.text = update.HPMax.ToString();
    }

    public Color changeImageColor(Image input, byte red, byte green, byte blue, byte alpha)
    {
        return input.color = new Color(red / 255, green / 255, blue / 255, alpha / 255);
    }
    //Highscore
    public void AddScore(int points)
    {
        currentScore += points;
       // Debug.Log($"Added {points} points! Total Score: {currentScore}");
    }

    public void AddScore(EnemyType enemyType)
    {
        int points = GetEnemyPoints(enemyType);
        AddScore(points);
    }

    public int GetEnemyPoints(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.RegularZombie:
                return regularZombiePoints;
            case EnemyType.HeavyZombie:
                return heavyZombiePoints;
            case EnemyType.Spitter:
                return spitterPoints;
            case EnemyType.DogZombie:
                return dogZombiePoints;
            case EnemyType.Boss1:
                return boss1Points;
            case EnemyType.Boss2:
                return boss2Points;
            case EnemyType.Boss3:
                return boss3Points;
            default:
                return 100;
        }
    }

    //public void EndGame()
    //{
    //    isGameActive = false;

    //    if (highscoreTable != null && highscoreTable.IsHighScore(currentScore))
    //    {
    //        highscoreTable.ShowHighScoreInput(currentScore, gameTime);
    //    }
    //}

    public void EnemyKilled(EnemyType enemyType = EnemyType.RegularZombie)
    {
        killCount++;
        AddScore(enemyType);
    }

    public void CompleteLevel()
    {
        //EndGame();
    }

    //Experience
    public void giveXP(int XP)
    {
        gameData.playerData.experience += XP;
        playerScript.UpdatePlayerUI();
    }
    public void levelUp()
    {
        if (playerScript == null) return;
        gameData.playerData = playerScript.playerData;
        float tempMaxHP = gameData.playerData.HPMax;
        if (gameData.playerData.experience >= gameData.playerData.levelUpCap)
        {
          //  Debug.Log("you have leveled up");
            gameData.playerData.level += 1;
            levelText.text = gameData.playerData.level.ToString();
            gameData.playerData.experience -= gameData.playerData.levelUpCap;
            gameData.playerData.levelUpCap *= increaseXpCap;
            FlashText(levelText, Color.yellow, 2);
            gameData.playerData.HPMax = playerStatUpgrade(tempMaxHP);
            playerScript.Heal((int)tempMaxHP / 4);
            gameData.playerData.HP = playerScript.playerData.HP;
            gameData.playerData.itemList.Clear();
            foreach(Pickups item in playerScript.playerData.itemList)
            {
                gameData.playerData.itemList.Add(item);
            }
            playerScript.UpdatePlayer(gameData.playerData);
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

       // Debug.Log("increase " + stat);

        return stat;
    }
    public float enemytatUpgrade(float stat)
    {
        float statMultiplier = 1.05f; //maybe do a system where it edits how fast the zombies addabt to the player.

        stat *= statMultiplier;

        return stat;
    }
    //Armor
    public void addArmor(int input)
    {
        for (int index = 0; index < input; index++)
        {
            tempArmor = Instantiate(armorPrefab, armorParent.transform);
            armors.Add(tempArmor);
        }
    }
    public void removeArmor()
    {
        if (armors.Count > 0)
        {
            Image lastArmor = armors[armors.Count - 1];

            armors.RemoveAt(armors.Count - 1);

            if (lastArmor != null)
                Destroy(lastArmor.gameObject);
        }
    }
    //DataManagement
    public void StartData()
    {
        gameData ??= new GameData();
        hubData ??= new GameData();
        if (DataManager.manager == null)
        {
            DataManager.manager = this;
        }
        if (playerScript != null)
        {
            playerSpawn = GameObject.FindWithTag("PlayerSpawn");
            playerScript.SwapCharacter(baseCharacter);
            Shooting.instance.changeGun(0);
        }
    }
    public void UpdateRun()
    {
        gameData.sceneIndex = sceneIndex;
        gameData.currentpickUps = pickUpObjects;
        gameData.player = player;
        gameData.playerData = playerScript.GetPlayerData();
        DataManager.instance.SaveRun(gameData);
    }
    public void LoadRun()
    {
        gameData = DataManager.instance.LoadRun();
        player = gameData.player;
    }
}

