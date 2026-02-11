using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    
    [SerializeField] int Health;
    
    [SerializeField] int faceTargetSpeed;

    Color colorOrg;

    bool playerInTrigger;

    Vector3 playerDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrg = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = GameManager.instance.player.transform.position - transform.position;

        if (playerInTrigger)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                faceTarget();
            }
               
        }
    }

    void faceTarget()
    {
        Quaternion rot= Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
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
    public void takeDamage(int amount)
    {
        Health -= amount;

        if(Health <= 0)
        {
            Destroy(gameObject);
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
        model.material.color= colorOrg;
    }

  
}
