using UnityEngine;

public class GameData
{
    public int sceneIndex;
    public GameManager gameManager;
    public GameObject player;
    public PlayerController playerScript;
    public Shooting gunData;
    public GameData()
    {
        gameManager = GameManager.instance;
        player = gameManager.player;
        playerScript = gameManager.playerScript;
        gunData = Shooting.instance;
        sceneIndex = GameManager.instance.sceneIndex;
    }
}
    
