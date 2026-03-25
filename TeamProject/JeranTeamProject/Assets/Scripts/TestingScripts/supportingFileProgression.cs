using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum progess
{

    get10Kills,get50kills, get100kills,get500kills, get1000kills, get40000kills,
    findAcommonWeapon,findAUncommon,findARare, findAPerfected, findAExotic, findASpecial,
    survive1Day, survive5days, survive10days, survive15days, completeArun,
    PerfectRun, masochist, 
}

public class supportGameProgression : MonoBehaviour
{
    PlayerController player;
    GameManager Manager;
    Shooting guns;
    int kills;
    public HashSet<progess> unlocked = new HashSet<progess>();

    //public void Awake()
    //{
    //    Manager = DataManager.manager;
    //    if (Manager != null && Manager.playerScript != null)
    //    {
    //        player = Manager.playerScript;
    //    }

    //}
    private void Start()
    {
        Manager = DataManager.manager;
        if (Manager != null && Manager.playerScript != null)
        {
            player = Manager.playerScript;
        }
    }

    public void Unlock(progess achievement)
    {
        if (!unlocked.Contains(achievement))
        {
            unlocked.Add(achievement);
          //  Debug.Log("Unlocked: " + achievement);
        }
    }

    public void getkills(int kill)
    {
        kills += kill;
        if(kills >= 10 && !unlocked.Contains(progess.get10Kills))
        {
          //  Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get10Kills);
            Manager.addDialog("you killed 10 enemies"); //the addition to the dialog is temp for testing, it will be updated for a notif bar that willpop up and slide away.
        }

        if (kills >= 50 && !unlocked.Contains(progess.get50kills))
        {
          //  Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get50kills);
            Manager.addDialog("you killed 50 enemies");
        }

        if (kills >= 100 && !unlocked.Contains(progess.get100kills))
        {
          //  Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get100kills);
            Manager.addDialog("you killed 100 enemies");
        }
        if (kills >= 500 && !unlocked.Contains(progess.get500kills))
        {
          //  Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get500kills);
            Manager.addDialog("you killed 500 enemies");
        }

    }

    public void FindItem()
    {
        for(int index = 0; index < guns.gunList.Count; index++)
        {
            if (guns.gunList[index] == GameObject.FindGameObjectWithTag("Common") && !unlocked.Contains(progess.findAcommonWeapon))
            {

               // Debug.LogWarning("*********achievement unlocked************");
                Unlock(progess.findAcommonWeapon);
                Manager.addDialog("you found a Common weapon");


            }

            if (guns.gunList[index] == GameObject.FindGameObjectWithTag("Uncommon") && !unlocked.Contains(progess.findAUncommon))
            {

               // Debug.LogWarning("*********achievement unlocked************");
                Unlock(progess.findAUncommon);
                Manager.addDialog("you found a Uncommon weapon");
            }

        }

    }

    public void surviveDays()
    {
        if(Manager.DAYs == 1 && !unlocked.Contains(progess.survive1Day))
        {
            Unlock(progess.survive1Day);
            Manager.addDialog("you survived 1 day");

        }
        if (Manager.DAYs == 5 && !unlocked.Contains(progess.survive5days))
        {
            Unlock(progess.survive5days);
            Manager.addDialog("you survived 1 day");

        }
    }
}