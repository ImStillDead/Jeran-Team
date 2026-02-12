using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    
    [SerializeField] int Health;
    [SerializeField] int Speed;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] int FOV;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;

    [SerializeField] GameObject spit;
    [SerializeField] float spitRate;
    [SerializeField] Transform spitPos;

    [SerializeField] int contactDamage;
    [SerializeField] float damageRate;
    Color colorOrg;

    float spitTimer;
    float roamTimer;
    float damageTimer;

    bool playerInTrigger;
    float roamTimer;
    float stoppingDistanceOrig;
    float angleToPlayer;
    Vector3 startingPos;
    Vector3 playerDir;
    void Start()
    {
        colorOrg = model.material.color;
        GameManager.instance.enemyBoardCount(1);
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
        agent.speed = Speed;
    }
    void Update()
    {
        playerDir = GameManager.instance.player.transform.position - transform.position;
        if (GameManager.instance.objectiveTimer > 0)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);
        }
        spitTimer += Time.deltaTime;
        roamTimer += Time.deltaTime;
        damageTimer += Time.deltaTime;
        if (playerInTrigger&& canSeePlayer())
        {
            agent.stoppingDistance = stoppingDistanceOrig;
            agent.SetDestination(GameManager.instance.player.transform.position);
        }
        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
        }
        if (playerInTrigger && !CanSeePlayer())
        {
            CheckRoam();
        }
        else if (!playerInTrigger)
        {
            CheckRoam();
            agent.stoppingDistance = stoppingDistOrig;

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                faceTarget();

                if (spitTimer >= spitRate)
                {
                    spitTimer = 0;
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

        IDamage playerDamage = GameManager.instance.player.GetComponent<IDamage>();
        if (playerDamage != null)
        {
            playerDamage.takeDamage(contactDamage);
        }
    }

    /*void checkRoam() 
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance+0.01f && roamTimer>= roamPauseTime)
        {
            roam();
        }    
    }*/

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 randPos = Random.insideUnitSphere * roamDist;
        randPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
    }

    /*bool canSeePlayer()
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
    }*/
    
    void CheckRoam()
    {
        if (agent.remainingDistance < 0.01f && roamTimer >= roamPauseTime)
        {
            Roam();
        }
    }
    
    bool CanSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(transform.position, playerDir, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            agent.stoppingDistance = stoppingDistanceOrig;
            if (angleToPlayer <= FOV && hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(GameManager.instance.player.transform.position);
                

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
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
        }
    }

    void shoot()
    {
        spitTimer = 0;
        Instantiate(spit, spitPos.position, transform.rotation);
    }
    public void takeDamage(int amount)
    {
        Health -= amount;
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (Health <= 0)
        {
            GameManager.instance.enemyBoardCount(-1);
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
