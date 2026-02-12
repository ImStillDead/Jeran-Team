using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    
    [SerializeField] int Health;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] int FOV;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;

    Color colorOrg;

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
    }
    void Update()
    {
        playerDir = GameManager.instance.player.transform.position - transform.position;
        if (GameManager.instance.objectiveTimer > 0)
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
        }
    }
    void CheckRoam()
    {
        if (agent.remainingDistance < 0.01f && roamTimer >= roamPauseTime)
        {
            Roam();
        }
    }
    void Roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 randPos = Random.insideUnitSphere * roamDist;
        randPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
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
    public void takeDamage(int amount)
    {
        Health -= amount;

        if(Health <= 0)
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
