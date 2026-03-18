using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance.playerSpawn != gameObject)
        {
            GameManager.instance.playerSpawn = gameObject;
            //GameManager.instance.playerScript.spawnPlayer();
        }
    }

}
