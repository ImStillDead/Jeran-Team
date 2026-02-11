using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    
    [SerializeField] int Health;
    [SerializeField] int Speed;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;

    [SerializeField] GameObject spit;
    [SerializeField] float spitRate;
    [SerializeField] Transform spitPos;

    [SerializeField] int contactDamage =10;
    [SerializeField] float damageRate =1f;

    Color colorOrg;

    float spitTimer;
    float roamTimer;
    float stoppingDistOrig;
    float damageTimer;

    bool playerInTrigger;

    Vector3 playerDir;
    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrg = model.material.color;
        //GameManager.instance.youWin();
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;

        agent.speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        spitTimer += Time.deltaTime;
        roamTimer += Time.deltaTime;
        damageTimer += Time.deltaTime;

        if (playerInTrigger&& canSeePlayer())
        {
            agent.SetDestination(GameManager.instance.player.transform.position);
            agent.stoppingDistance = stoppingDistOrig;

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                faceTarget();

                if (spitTimer >= 1f)
                {
                    shoot();
                }

                if(damageTimer >= damageRate)
                {
                    damagePlayer();
                }
            }
        }
        else
        {
            agent.stoppingDistance = 0;
            checkRoam();
        }
    }
    void damagePlayer()
    {
        damageTimer = 0;

        IDamage playerDamage= GameManager.instance.player.GetComponent<IDamage>();
        if (playerDamage != null)
        {
            playerDamage.takeDamage(contactDamage);
        }
    }

    void checkRoam() 
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance+0.01f && roamTimer>= roamPauseTime)
        {
            roam();
        }    
    }

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = UnityEngine.Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(ranPos, out hit, roamDist, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    bool canSeePlayer()
    {
        if (GameManager.instance.player == null) return false;

        playerDir = (GameManager.instance.player.transform.position - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            Debug.DrawRay(transform.position, playerDir * hit.distance, Color.red);

            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, playerDir * hit.distance, Color.green);
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
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
            agent.stoppingDistance = 0;
            roamTimer = roamPauseTime;
        }
    }

    void shoot()
    {
        spitTimer = 0;
        Instantiate(spit, spitPos.position, transform.rotation);
    }

    private void Instantiate(GameObject spit, object position, Quaternion rotation)
    {
        throw new NotImplementedException();
    }

    public void takeDamage(int amount)
    {
        Health -= amount;
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (Health <= 0)
        {
            //GameManager.instance.updateGameGoal(-1);
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
