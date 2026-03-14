using UnityEngine;

public class GameData
{
    public int sceneIndex;
    public GameObject player;
    public PlayerController playerScript;
    public GameData()
    {
        if(GameManager.instance != null)
        {
            player = GameManager.instance.player;
            playerScript = GameManager.instance.playerScript;
            sceneIndex = GameManager.instance.sceneIndex;
        }
    }
}
    
