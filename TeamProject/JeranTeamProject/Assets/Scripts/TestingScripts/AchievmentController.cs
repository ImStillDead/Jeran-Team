using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour
{
    [Header("contollers")]
    public GameManager manager;
    public PlayerController player;
    public supportGameProgression achievements;
    public Shooting currentgun;
    
    [Header("positions of the prefab")]
    [SerializeField] Transform achievementParent;   
    [SerializeField] Transform achievementTargetLocation;


    [Header("images and image holder")]
    [SerializeField] GameObject achievement_bar;
    [SerializeField] List<Sprite> emblemAcheivementList;
    [SerializeField] Image emblemImage;
    [SerializeField] Image boarderTierColor;
    [SerializeField] TMP_Text AchievementName;
    [SerializeField] TMP_Text description;


    private int daysSurvived;
    public bool activate;

    void Start()
    {
        manager = GameManager.instance;          // get manager instance
        if (manager != null)
        {
            player = manager.playerScript;       // get player controller
            achievements = manager.prog;         // get progression
            if (player != null)
                currentgun = player.Gun;         // get current gun
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerKills();

        if (activate)
        {
            MoveAchievement(4f);  
            activate = false;
        }

    }

    void PlayerKills()
    {
        

       giveEmblemColor(achievements.getkills(manager.killCount));
        emblemImage.sprite = emblemAcheivementList[0];
        activate = true;
    }

    void giveEmblemColor(int number)
    {
        switch (number)
        {
            case 1:
                boarderTierColor.color = Color.gray;
                break;
            case 2:
                boarderTierColor.color = Color.lightGreen;
                break;
            case 3:
                boarderTierColor.color = Color.lightBlue;
                break;
            case 4:
                boarderTierColor.color = Color.lightYellow;
                break;
            case 5:
                boarderTierColor.color = Color.red;
                break;
            case 6:
                boarderTierColor.color = Color.gold;
                break;

        }
    }

    void MoveAchievement(float time)
    {

         StartCoroutine(pullBarDown(time));
        
    }

    IEnumerator pullBarDown(float duration)
    {
        Vector3 startPos = achievementParent.position;
        Vector3 endPos = achievementTargetLocation.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            achievement_bar.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        achievement_bar.transform.position = endPos;

  
        yield return new WaitForSeconds(1f);

    
        elapsed = 0f;
        while (elapsed < duration)
        {
            achievement_bar.transform.position = Vector3.Lerp(endPos, startPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        achievement_bar.transform.position = startPos;
    }


}
