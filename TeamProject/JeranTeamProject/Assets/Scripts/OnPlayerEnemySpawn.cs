using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class OnPlayerEnemySpawn : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] int spawnAmmount;
    [SerializeField] int spawnRate;
    [SerializeField] int spawnDist;
    [SerializeField] public int spawnMax;
    [SerializeField] bool devSpawnOff;
    public int spawnCount;
    float spawnTimer;
    bool startSpawner;
    bool inGame;
    private void Start()
    {
        spawnCount = 0;
        inGame = false;
        
    }
    void Update()
    {
        if (devSpawnOff)
        {
            inGame = false;
        }
        else if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            inGame = true;
        }
        if (spawnCount < spawnMax && inGame)
        {
            startSpawner = true;
        }
        else
        {
            startSpawner = false;
        }
        if (startSpawner) 
        { 
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnRate)
            {
                for (int i = 0; i < spawnAmmount; i++)
                {
                    spawn();
                }
                spawnTimer = 0;
            }
        }
    }
    void spawn()
    {
        spawnCount++;
        Vector3 randPos = Random.insideUnitSphere * spawnDist;
        randPos += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randPos, out hit, spawnDist, 1);
        int randomObject = Random.Range(0, spawnObjects.Count);
        if(spawnObjects.Count == 1)
        {
            randomObject = 0;
        }
        Instantiate(spawnObjects[randomObject], hit.position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
    }
}
