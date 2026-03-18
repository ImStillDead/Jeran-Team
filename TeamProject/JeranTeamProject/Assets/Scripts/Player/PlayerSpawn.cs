using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameManager.instance.playerSpawn != gameObject)
        {
            GameManager.instance.playerSpawn = gameObject;
            //GameManager.instance.playerScript.spawnPlayer();
        }
    }

}
