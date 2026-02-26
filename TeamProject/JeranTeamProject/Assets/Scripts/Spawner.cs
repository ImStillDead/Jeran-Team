using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] int spawnAmmount;
    [SerializeField] int spawnRate;
    [SerializeField] int spawnDist;

    int spawnCount;
    float spawnTimer;
    bool startSpawner;
    void Start()
    {
        
    }
    void Update()
    {
        if (startSpawner)
        {
            spawnTimer += Time.deltaTime;
            if (spawnCount < spawnAmmount && spawnTimer > spawnRate)
            {
                spawn();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.instance.enemyCount == 0)
            {
                GameManager.instance.enemyBoardCount(spawnAmmount);
                startSpawner = true;
            }
            if (GameManager.instance.canSpawn == true && spawnCount <= spawnAmmount)
            {
                GameManager.instance.enemyBoardCount(spawnAmmount);
                spawnCount = 0;
                startSpawner = true;
            }
        }
    }
    void spawn()
    {
        spawnTimer = 0;
        spawnCount++;

        Vector3 randPos = Random.insideUnitSphere * spawnDist;
        randPos += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randPos, out hit, spawnDist, 1);

        Instantiate(spawnObject, hit.position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
    }

}
