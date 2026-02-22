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
    [SerializeField] Transform neckPivot;
    [SerializeField] int neckRotationSpeed;

    [SerializeField] int contactDamage;
    [SerializeField] float damageRate;
    [SerializeField] int meleeDist;
    
    Color colorOrg;
    GameObject door;
    float spitTimer;
    float roamTimer;
    float damageTimer;
    bool doorHit;
    bool playerInTrigger;
    float stoppingDistanceOrig;
    float angleToPlayer;
    Vector3 startingPos;
    Vector3 playerDir;

    EnemyHealthBar healthBar;
    void Start()
    {
        colorOrg = model.material.color;
        GameManager.instance.enemyBoardCount(1);
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
        agent.speed = Speed;

        healthBar = GetComponentInChildren<EnemyHealthBar>();
        if(healthBar != null)
        {
            healthBar.Setup(Health);
        }

    }
    void Update()
    {
        if (GameManager.instance.objectiveTimer >= 3)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);
        }

        if (agent.remainingDistance < 0.5f)
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
        }
        else
        {
            spitTimer += Time.deltaTime;
            damageTimer += Time.deltaTime;
        }
       /* if (playerInTrigger && CanSeePlayer())
        {
            agent.stoppingDistance = stoppingDistanceOrig;
            agent.SetDestination(GameManager.instance.player.transform.position);
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                faceTarget();

                if (spitTimer >= spitRate)
                {
                    spitTimer = 0;
                    shoot();
                }

                if (damageTimer >= damageRate)
                {
                    damagePlayer();
                }
            }
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
            agent.stoppingDistance = 0;
            CheckRoam();
        }*/
    }
    
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
    void CheckRoam()
    {
        if (agent.remainingDistance < 0.5f && roamTimer >= roamPauseTime)
        {
            roam();
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
                if (spitTimer >= spitRate && agent.remainingDistance >= meleeDist)
                {
                    shoot();
                }

                neckRotate();

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                    IDamage playerDamage = hit.collider.GetComponent<IDamage>();
                    if (damageTimer >= damageRate && agent.remainingDistance <= meleeDist)
                    {
                        damageTimer = 0;
                        playerDamage.takeDamage(contactDamage);
                    }
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;

    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
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
        Instantiate(spit, spitPos.position, neckPivot.rotation);
        if (spitTimer >= spitRate)
        {
            shoot();
        }
    }
    public void takeDamage(int amount)
    {
        Health -= amount;

        if(healthBar != null)
        {
            healthBar.UpdateHealth(Health);
        }


        agent.SetDestination(GameManager.instance.player.transform.position);

        if (Health <= 0)
        {
            GameManager.instance.enemyBoardCount(-1);
            Destroy(gameObject);
            GameManager.instance.killCount++;
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


    void neckRotate()
    {
        Vector3 directionToPlayer = GameManager.instance.player.transform.position - neckPivot.position;

        float horizontalAngel= Mathf.Atan2(directionToPlayer.x, directionToPlayer.z)*Mathf.Rad2Deg;

        float verticalAngle = -Mathf.Atan2(directionToPlayer.y, new Vector3(directionToPlayer.x, 0, directionToPlayer.z).magnitude) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(verticalAngle, horizontalAngel, 0);
        neckPivot.rotation=Quaternion.RotateTowards(neckPivot.rotation, targetRotation, neckRotationSpeed* Time.deltaTime);
    }
  
}
