using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerDoor : MonoBehaviour
{
    [SerializeField] GameObject model;
    bool enemyInTrigger;

    [SerializeField] GameObject spawnObject;
    [SerializeField] Transform spawnPos;
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
        if (enemyInTrigger)
        {
            model.SetActive(false);
        }
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
        if (other.CompareTag("Enemy"))
        {
            enemyInTrigger = true;
        }
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.enemyCount == 0)
            {
                GameManager.instance.enemyBoardCount(spawnAmmount);
                startSpawner = true;
            } else if (GameManager.instance.canSpawn == true && spawnCount <= spawnAmmount)
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
        randPos += spawnPos.transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randPos, out hit, spawnDist, 1);

        Instantiate(spawnObject, spawnPos.transform.position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
    }
 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            model.SetActive(true);
            enemyInTrigger = false;
        }
    }
}
