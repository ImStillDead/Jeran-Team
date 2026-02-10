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

    int magsize;
    int maxMagsize;

    public Image PlayerHP_bar;
    public GameObject playerDamageFlash;


    public GameObject player;
    public playerController playScritp;
    public bool isPaused;

    float timeScaleOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;

        player = GameObject.FindWithTag("Player");
        playScritp = player.GetComponent<playerController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void update_ammocount(int ammo)
    {
        maxMagsize = ammo;
        maxMagsize_text.text = maxMagsize_text.ToString();


    }



}
