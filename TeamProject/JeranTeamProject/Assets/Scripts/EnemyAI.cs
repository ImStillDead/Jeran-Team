using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [Header("Zombie Stats")]
    [SerializeField] int HP;
    [SerializeField] int Speed;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int maxLifeTimer;
    [SerializeField] GameObject enemyHPBar;
    public Image enemyHealth;

    [Header("Zombie Vision/roam")]
    [SerializeField] int FOV;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    [SerializeField] float forgetPlayerTime = 5f;
    public bool isRoaming;

    [Header("Zombie Spit")]
    [SerializeField] GameObject spit;
    [SerializeField] float spitRate;
    [SerializeField] Transform spitPos;
    [SerializeField] Transform neckPivot;
    [SerializeField] int neckRotationSpeed;
    [SerializeField] bool canSpit = true;
    [Header("Zombie Damage")]
    [SerializeField] int contactDamage;
    [SerializeField] float damageRate;
    [SerializeField] int meleeDist;
    [Header("Zombie Exp")]
    [SerializeField] int killReward;
    [SerializeField] int experienceReward;
    [Header("Showcase Mode")]
    [SerializeField] bool showcaseMode = false;

    bool hasSpottedPlayer;
    float timeSinceLastSight;
   
    float inUseTimer;
    Color colorOrg;
    int HPOrigin;
    float spitTimer;
    float roamTimer;
    float damageTimer;

    bool playerInTrigger = false;
    float stoppingDistanceOrig;
    float angleToPlayer;
    Vector3 startingPos;
    Vector3 playerDir;

    GameManager manager;
    PlayerController player;

    void Start()
    {
        manager = GameManager.instance;
        player = GameManager.instance.player.GetComponent<PlayerController>();
        colorOrg = model.material.color;
        HPOrigin = HP;
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
        agent.speed = Speed;
        hasSpottedPlayer = false;
        timeSinceLastSight = 0f;

        if (showcaseMode)
        {
            agent.isStopped = true;
            agent.enabled = false; // Completely disable navmesh agent
        }
        else if (!isRoaming)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);
        }
    }

    void Update()
    {
        if (showcaseMode)
        {
            return;
        }

        if (GameManager.instance.objectiveTimer >= 3)
        {
            agent.SetDestination(GameManager.instance.player.transform.position);
        }

        if (enemyHPBar != null) manager.guiAlwaysFacePlayer(enemyHPBar);


        bool canSeePlayer = CanSeePlayer();

        if (canSeePlayer)
        {

            hasSpottedPlayer = true;
            timeSinceLastSight = 0f;
            playerInTrigger = true;
            enemyHPBar.SetActive(true);


            spitTimer += Time.deltaTime;
            damageTimer += Time.deltaTime;
            inUseTimer = 0;
        }
        else
        {

            if (hasSpottedPlayer)
            {
                timeSinceLastSight += Time.deltaTime;

                if (timeSinceLastSight < forgetPlayerTime)
                {

                    agent.SetDestination(GameManager.instance.player.transform.position);


                    if (agent.velocity.magnitude > 0.1f)
                    {
                        transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
                    }
                }
                else
                {

                    hasSpottedPlayer = false;
                    playerInTrigger = false;
                    enemyHPBar.SetActive(false);
                    agent.stoppingDistance = 0;
                }
            }


            if (!hasSpottedPlayer)
            {
                if (agent.remainingDistance < 0.5f)
                {
                    roamTimer += Time.deltaTime;
                }

                CheckRoam();

                if (!isRoaming)
                {
                    inUseTimer += Time.deltaTime;
                }
            }
        }


        if (inUseTimer > maxLifeTimer && !isRoaming && !hasSpottedPlayer)
        {
            GameManager.instance.enemyBoardCount(-1);
            Destroy(gameObject);
        }
    }

    void roam()
    {
        if (showcaseMode) return;

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
        if (showcaseMode) return;

        if (agent.remainingDistance < 0.5f && roamTimer >= roamPauseTime)
        {
            roam();
        }
    }

    bool CanSeePlayer()
    {
        if (showcaseMode) return false;

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

                if (canSpit && spitTimer >= spitRate && agent.remainingDistance >= meleeDist)
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
        if (showcaseMode) return;
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            hasSpottedPlayer = true;
            enemyHPBar.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (showcaseMode) return;
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    void shoot()
    {
        if (showcaseMode || !canSpit) return;
        spitTimer = 0;
        Instantiate(spit, spitPos.position, transform.rotation);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (enemyHealth != null)
        {
            enemyHealth.fillAmount = (float)HP / HPOrigin;
        }

        agent.SetDestination(GameManager.instance.player.transform.position);

        hasSpottedPlayer = true;
        timeSinceLastSight = 0f;

        if (HP <= 0)
        {
            GameManager.instance.enemyBoardCount(-1);
            GameManager.instance.killCount++;

            player.addPlayerMoney(killReward);
            manager.giveXP(experienceReward);
            player.updatePlayerUI();
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
        model.material.color = colorOrg;
    }

    void neckRotate()
    {
        if (showcaseMode) return;
        if (neckPivot == null) return;

        Vector3 directionToPlayer = GameManager.instance.player.transform.position - neckPivot.position;

        float horizontalAngel = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;
        float verticalAngle = -Mathf.Atan2(directionToPlayer.y, new Vector3(directionToPlayer.x, 0, directionToPlayer.z).magnitude) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(verticalAngle, horizontalAngel, 0);
        neckPivot.rotation = Quaternion.RotateTowards(neckPivot.rotation, targetRotation, neckRotationSpeed * Time.deltaTime);
    }
}