using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum progess
{
    doDamage,
    get10Kills, get100kills, get1000kills,
    findAcommonWeapon,findAUncommon,findARare, findAPerfected, findAExotic,



}

public class supportGameProgression : MonoBehaviour
{
    PlayerController player;
    GameManager Manager;
    private HashSet<progess> unlocked = new HashSet<progess>();

    public void Start()
    {
        Manager = GameManager.instance;
        player = Manager.player.GetComponent<PlayerController>();

    }

    public void Unlock(progess achievement)
    {
        if (!unlocked.Contains(achievement))
        {
            unlocked.Add(achievement);
            Debug.Log("Unlocked: " + achievement);
        }
    }

    public void get10kills(int kills)
    {
        if(kills >= 10 && !unlocked.Contains(progess.get10Kills))
        {
            Debug.LogWarning("*********achievement unlocked************");
            Unlock(progess.get10Kills);
            Manager.addDialog("you killed 10 enemies");
        }
    }



}