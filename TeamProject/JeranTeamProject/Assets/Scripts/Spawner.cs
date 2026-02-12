using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] int HP;
    //[SerializeField] Renderer model;
    [SerializeField] Transform spawn;
    [SerializeField] GameObject enemy;
    //[SerializeField] GameObject door;
    [SerializeField] int spawnRate;
    //Color colorOrg;
    int HPOrg;
    float spawnTimer;
    bool playerInTrigger;
    /*public void takeDamage(int ammount)
    {
        HP -= ammount;
        if (HP > 0) { 
            door.GetComponent<MeshRenderer>().enabled = false;
            door.GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }
    IEnumerator FlashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrg;
    }*/
    void Start()
    {
        HPOrg = HP;

    }
    void Update()
    {
        if (playerInTrigger)
        {
            spawnTimer += Time.deltaTime;
        }
        if (GameManager.instance.canSpawn && spawnTimer >= spawnRate)
        {
            spawnTimer = 0;
            spawnEnemy();
        }
    }
    private GameObject spawnEnemy()
    {
        return Instantiate(enemy, spawn.position, transform.rotation);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
    /*public void fixDoor()
    {
        HP = HPOrg;
        door.GetComponent<MeshRenderer>().enabled = true;
        door.GetComponent<Collider>().isTrigger = false;
    }*/
}
