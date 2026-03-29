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

    public int getkills(int kill)
    {
        int progression = 0;

        kills += kill;
        if (kills >= 10 && !unlocked.Contains(progess.get10Kills))
        {
            //  Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get10Kills);
            progression = 1;
        }

        if (kills >= 50 && !unlocked.Contains(progess.get50kills))
        {
            //  Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get50kills);
            progression = 2;

        }

        if (kills >= 100 && !unlocked.Contains(progess.get100kills))
        {
            //  Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get100kills);
            progression = 3;
        }
        if (kills >= 500 && !unlocked.Contains(progess.get500kills))
        {
            //  Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get500kills);
            progression = 4;

        }
        if (kills >= 1000 && !unlocked.Contains(progess.get1000kills))
        {
            //  Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get1000kills);
            progression = 5;
        }
        if (kills >= 40000 && !unlocked.Contains(progess.get40000kills))
        {
            //  Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get40000kills);
            progression = 6;
        }

        return progression;
    }

    public int FindItem()
    {
        int progression = 0;

        for(int index = 0; index < guns.gunList.Count; index++)
        {
            if (guns.gunList[index] == GameObject.FindGameObjectWithTag("Common") && !unlocked.Contains(progess.findAcommonWeapon))
            {

                Unlock(progess.findAcommonWeapon);
                Manager.addDialog("you found a Common weapon");
                progression = 1;

            }

            if (guns.gunList[index] == GameObject.FindGameObjectWithTag("Uncommon") && !unlocked.Contains(progess.findAUncommon))
            {
                Unlock(progess.findAUncommon);
                Manager.addDialog("you found a Uncommon weapon");
                progression = 2;

            }

            if (guns.gunList[index] == GameObject.FindGameObjectWithTag("Rare") && !unlocked.Contains(progess.findARare))
            {
                Unlock(progess.findARare);
               
                progression = 3;

            }

            if (guns.gunList[index] == GameObject.FindGameObjectWithTag("Perfected") && !unlocked.Contains(progess.findAPerfected))
            {
                Unlock(progess.findAPerfected);
                Manager.addDialog("you found a Uncommon weapon");
                progression = 4;
            }

            if (guns.gunList[index] == GameObject.FindGameObjectWithTag("Perfected") && !unlocked.Contains(progess.findAPerfected))
            {
                Unlock(progess.findAPerfected);
                Manager.addDialog("you found a Uncommon weapon");
                progression = 5;
            }
            if (guns.gunList[index] == GameObject.FindGameObjectWithTag("Exotic") && !unlocked.Contains(progess.findAExotic))
            {
                Unlock(progess.findAExotic);
                Manager.addDialog("you found a Uncommon weapon");
                progression = 6;
            }
            if (guns.gunList[index] == GameObject.FindGameObjectWithTag("Special") && !unlocked.Contains(progess.findASpecial))
            {
                Unlock(progess.findASpecial);
                Manager.addDialog("you found a Uncommon weapon");
                progression = 7;
            }

        }


        return progression;
    }

    public void surviveDays()
    {
        if (Manager.DAYs == 1 && !unlocked.Contains(progess.survive1Day))
        {
            Unlock(progess.survive1Day);
            Manager.addDialog("you survived 1 day");

        }
        if (Manager.DAYs == 5 && !unlocked.Contains(progess.survive5days))
        {
            Unlock(progess.survive5days);
            Manager.addDialog("you survived 1 day");
        }
        if (Manager.DAYs == 10 && !unlocked.Contains(progess.survive10days))
        {
            Unlock(progess.survive10days);
            Manager.addDialog("you survived 1 day");
        }
        if (Manager.DAYs == 15 && !unlocked.Contains(progess.survive15days))
        {
            Unlock(progess.survive15days);
            Manager.addDialog("you survived 1 day");
        }
        if (Manager.DAYs == 20 && !unlocked.Contains(progess.completeArun))
        {
            Unlock(progess.completeArun);
            Manager.addDialog("you survived 1 day");
        }


    }
}