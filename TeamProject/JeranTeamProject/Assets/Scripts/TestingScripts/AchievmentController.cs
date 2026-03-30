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
    public string NameOfAchievement;
    public string descriptionOfAchievement;
    private int lastActivatedTier;

    void Start()
    {
        if (manager == null)
            return;

        // check prog is assigned
        if (manager.prog == null)
        {
            Debug.LogError("GameManager.prog is not assigned!");
        }

        AchievementName.text = "this is a test";

    }

    // Update is called once per frame
    void Update()
    {


        if (activate)
        {
            MoveAchievement(2f);  
            activate = false;
        }

    }

    public void PlayerKills()
    {
        if (manager == null || manager.prog == null)
        {
            Debug.LogError("Achievements or Manager is NULL");
            return;
        }

        int currentTier = manager.prog.getkills(); // highest unlocked tier

        // Only trigger if the tier is higher than the last activated
        if (currentTier > 0 && currentTier != lastActivatedTier)
        {
            lastActivatedTier = currentTier; // update counter

            // Set achievement text based on the tier
            switch (currentTier)
            {
                case 1:
                    NameOfAchievement = "Kill 10 enemies";
                    descriptionOfAchievement = "Kill 10 of any type of enemy";
                    break;
                case 2:
                    NameOfAchievement = "Kill 50 enemies";
                    descriptionOfAchievement = "Kill 50 of any type of enemy";
                    break;
                case 3:
                    NameOfAchievement = "Kill 100 enemies";
                    descriptionOfAchievement = "Kill 100 of any type of enemy";
                    break;
                case 4:
                    NameOfAchievement = "Kill 500 enemies";
                    descriptionOfAchievement = "Kill 500 of any type of enemy";
                    break;
                case 5:
                    NameOfAchievement = "Kill 1000 enemies";
                    descriptionOfAchievement = "Kill 1000 of any type of enemy";
                    break;
                case 6:
                    NameOfAchievement = "Kill 40000 enemies";
                    descriptionOfAchievement = "Kill 40000 of any type of enemy";
                    break;
            }

            // Update UI
            AchievementName.text = NameOfAchievement;
            description.text = descriptionOfAchievement;
            giveEmblemColor(currentTier);

            if (emblemAcheivementList != null && emblemAcheivementList.Count > 0)
                emblemImage.sprite = emblemAcheivementList[0];

            // Activate animation
            activate = true;
        }
    }

    void giveEmblemColor(int number)
    {
        switch (number)
        {
            case 1:
                boarderTierColor.color = new Color32(128, 128, 128, 255); // gray
                break;
            case 2:
                boarderTierColor.color = new Color32(144, 238, 144, 255); // light green
                break;
            case 3:
                boarderTierColor.color = new Color32(173, 216, 230, 255); // light blue
                break;
            case 4:
                boarderTierColor.color = new Color32(255, 255, 224, 255); // light yellow
                break;
            case 5:
                boarderTierColor.color = new Color32(255, 0, 0, 255); // red
                break;
            case 6:
                boarderTierColor.color = new Color32(255, 215, 0, 255); // gold
                break;
        }
    }

    void MoveAchievement(float time)
    {
        float drop = time / 4;

         StartCoroutine(pullBarDown(drop,time));
        
    }

    IEnumerator pullBarDown(float dropspeed, float stayAmount)
    {
        Vector3 startPos = achievementParent.position;
        Vector3 endPos = achievementTargetLocation.position;
        float elapsed = 0f;

        while (elapsed < dropspeed)
        {
            achievement_bar.transform.position = Vector3.Lerp(startPos, endPos, elapsed / dropspeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        achievement_bar.transform.position = endPos;

  
        yield return new WaitForSeconds(stayAmount);

    
        elapsed = 0f;
        while (elapsed < dropspeed)
        {
            achievement_bar.transform.position = Vector3.Lerp(endPos, startPos, elapsed / dropspeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        achievement_bar.transform.position = startPos;

    }


}
