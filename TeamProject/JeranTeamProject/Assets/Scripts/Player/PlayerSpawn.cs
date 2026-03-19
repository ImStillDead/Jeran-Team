using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        if (DataManager.manager.playerSpawn != gameObject)
        {
            DataManager.manager.playerSpawn = gameObject;
            //GameManager.instance.playerScript.spawnPlayer();
        }
    }

}
