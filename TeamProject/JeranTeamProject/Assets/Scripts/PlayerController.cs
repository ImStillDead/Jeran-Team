using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour, IDamage, IPickup, IGunPickup, IDash
{
    public PlayerController instance;

    [SerializeField] CharacterController playerController;
    [SerializeField] LayerMask ignoreLayer;

    [Header("Player Stats")]
    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int interactDis;
    [SerializeField] int enemyViewDis;
    [SerializeField] int gravity;

    [Header("Dash Settings")]
    [SerializeField] float dashForce = 30f;
    [SerializeField] float dashDur = 0.2f;
    [SerializeField] float dashCD = 1f; //CD = cooldown

    [Header("Sliding Settings")]
    [SerializeField] bool canSlide = true;
    [SerializeField] float slideSpeed = 12f;
    [SerializeField] float slideDur = 1.5f;
    [SerializeField] float slideYScale = 0.5f;
    [SerializeField] float slideCD = 1f;

    [Header("Other Settings")]
    [SerializeField] Transform weaponPos;
    [SerializeField] GameObject firstPersonCamera;
    [SerializeField] GameObject thirdPersonCamera;
    [SerializeField] GameObject torch;
    [SerializeField] List<Pickups> itemList = new List<Pickups>();
    [SerializeField] AudioSource aud;
    Pickups activePick;
    int HPMax;
    int jumpCount;
    int invPos;
    int gunPos;
    int itemIndex;
    float boostTime;
    int tempOrginDmg;
    bool dmgBoosting;
    int tempOrginSpeed;
    bool isFirstPerson;
    bool torchActive;
    Vector3 moveDir;
    Vector3 playerVel;

    //sliding varibles
    private Dashing dashingComponent;
    private bool isSliding;
    private float slideTimer;
    private float slideCDTimer;
    private float originalHeight;
    private float originalYScale;
    private Vector3 originalCenter;
    private CharacterController characterController;

    // Start and Update Functions
    void Start()
    {
        spawnPlayer();
        if (DataManager.instance.basePlayerStats.Count < 1)
        {
            setBaseStats();
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if(playerController == null)
        {
            playerController = instance.GetComponent<CharacterController>();
        }

        SetupDashing(); 
        SetupSliding();

        isFirstPerson = true;
        HPMax = HP;
        updatePlayerUI();
    }
    void Update()
    {
        Movement();
        WeaponRotate();
        Sprint();
    }

    // Movement and Button Interactions
    void Movement()
    {
        HandleSliding();

        if (IsDashing()) // checks dashing
        {
            playerController.Move(playerVel * Time.deltaTime);
            playerVel.y -= gravity * Time.deltaTime;
            return;
        }

        if (isSliding) // checks sliding
        {
            playerController.Move(playerVel * Time.deltaTime);
            playerVel.y -= gravity * Time.deltaTime;

            if (playerController.isGrounded)
            {
                playerVel = Vector3.zero;
            }
            return;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            Debug.Log($"Input detected - H:{horizontal}, V:{vertical}");
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + (Input.GetAxis("Vertical") * transform.forward);
        playerController.Move(moveDir * speed * Time.deltaTime);
        playerController.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;
        if (playerController.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }
        
        Jump();
        ChangeActiveInventory();
        CameraToggle();
        Interact();
        useItem();
        ToggleTorch();
    }

    void SetupDashing()
    {
        dashingComponent = GetComponent<Dashing>();
        if (dashingComponent == null)
        {
            dashingComponent = gameObject.AddComponent<Dashing>();
        }

        dashingComponent.dashForce = dashForce;
        dashingComponent.dashDur = dashDur;
        dashingComponent.dashCD = dashCD;
    }

    void SetupSliding()
    {
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
        originalCenter = characterController.center;
        originalYScale = transform.localScale.y;
    }

    void HandleSliding()
    {
        // Update cooldown timer
        if (slideCDTimer > 0)
            slideCDTimer -= Time.deltaTime;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool isMoving = (horizontalInput != 0 || verticalInput != 0);

        if (Input.GetButtonDown("Slide") && canSlide && !isSliding && isMoving && slideCDTimer <= 0)
        {
            StartSlide();
        }

        if (Input.GetButtonDown("Slide")&& isSliding)
        {
            StopSlide();
        }

        // Handle sliding movement
        if (isSliding)
        {
            SlidingMovement();
        }
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDur;

        characterController.height = originalHeight * slideYScale;
        characterController.center = new Vector3(originalCenter.x, originalCenter.y * slideYScale, originalCenter.z);

    }

    void SlidingMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;

        if (moveDirection.magnitude > 0.1f)
        {
            playerController.Move(moveDirection.normalized * slideSpeed * Time.deltaTime);
        }

        slideTimer -= Time.deltaTime;

        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    void StopSlide()
    {
        isSliding = false;
        slideCDTimer = slideCD;

        characterController.height = originalHeight;
        characterController.center = originalCenter;
    }

    public bool IsSliding()
    {
        return isSliding;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(int newSpeed)
    {
        speed = newSpeed;
    }

    public int GetJumpSpeed()
    {
        return jumpSpeed;
    }

    public void SetJumpSpeed(int newJumpSpeed)
    {
        jumpSpeed = newJumpSpeed;
    }

    // IDash Implementation
    public void StartDash()
    {
        if (dashingComponent != null)
            dashingComponent.StartDash();
    }

    public bool IsDashing()
    {
        return dashingComponent != null && dashingComponent.IsDashing();
    }

    public float GetDashRemainingCooldown()
    {
        return dashingComponent != null ? dashingComponent.GetDashRemainingCooldown() : 0f;
    }


    void CameraToggle()
    {
        if (Input.GetButtonDown("ToggleCamera"))
        {
            if (isFirstPerson)
            {
                weaponPos.transform.Rotate(-4, 4, 0);
                thirdPersonCamera.SetActive(true);
                firstPersonCamera.SetActive(false);
                isFirstPerson = false;
            }
            else
            {
                weaponPos.transform.Rotate(4, -4, 0);
                firstPersonCamera.SetActive(true);
                thirdPersonCamera.SetActive(false);
                isFirstPerson = true;
            }
        }
    }
    void Jump()
    {
        if (isSliding)
            return;

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }
    }
    void Sprint()
    {
        if (IsDashing() || isSliding)
            return;

        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        } else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }
    void ToggleTorch()
    {
        if (Input.GetButtonDown("Torch"))
        {
            if (torchActive == true)
            {
                torch.SetActive(false);
                torchActive = false;
            }
            else
            {
                torch.SetActive(true);
                torchActive = true;
            }
        }
    }
    void ChangeActiveInventory()
    {
        // Item Swap
        if (Input.GetButtonDown("Swap"))
        {
            if (invPos >= itemList.Count - 1)
            {
                invPos = 0;
            }
            else
            {
                invPos++;
            }
            changeItem(invPos);
        }
        // Weapon Scroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (gunPos >= Shooting.instance.gunList.Count - 1)
            {
                gunPos = 0;
            }
            else
            {
                gunPos++;
            }
            updateGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (gunPos <= 0)
            {
                gunPos = Shooting.instance.gunList.Count - 1;
            }
            else
            {
                gunPos--;
            }
            updateGun();
        }
        // Weapon Select 1-5
        if (Input.GetButtonDown("Weapon1"))
        {
            gunPos = 0;
            Shooting.instance.changeGun(gunPos);
        }
        else if (Input.GetButtonDown("Weapon2"))
        {
            gunPos = 1;
            Shooting.instance.changeGun(gunPos);
        }
        else if (Input.GetButtonDown("Weapon3"))
        {
            gunPos = 2;
            Shooting.instance.changeGun(gunPos);
        }
        else if (Input.GetButtonDown("Weapon4"))
        {
            gunPos = 3;
            Shooting.instance.changeGun(gunPos);
        }
        else if (Input.GetButtonDown("Weapon5"))
        {
            gunPos = 4;
            Shooting.instance.changeGun(gunPos);
        }
    }
    void Interact()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Vector3 origin = Camera.main.transform.position;
            Vector3 direction = Camera.main.transform.forward;

            Debug.DrawRay(origin, direction * interactDis, Color.mediumVioletRed);

            if (Physics.Raycast(origin, direction, out RaycastHit hitInter, interactDis))
            {
                if (hitInter.collider.TryGetComponent<iInteract>(out var interactable))
                {
                    Debug.Log($"Interacting with {hitInter.collider.name}");
                    interactable.Interacted();
                }
            }
        }

    }

    // Gun interactions
    public void GetGunStats(GunStats gun)
    {
        if (Shooting.instance.gunList.Contains(gun))
        {

        }
        else
        {
            Shooting.instance.gunList.Add(gun);
            DataManager.instance.currentGuns.Add(gun);
            gunPos = Shooting.instance.gunList.Count - 1;
            if (Shooting.instance.gunList.Count == 1)
            {
                Shooting.instance.changeGun(gunPos);
            }
        }

    }
    public void updateGun()
    {
        Shooting.instance.changeGun(gunPos);
    }
    void WeaponRotate()
    {
        if (isFirstPerson)
        {
            weaponPos.transform.rotation = firstPersonCamera.transform.rotation;
            interactDis = 3;
        }
        else
        {
            weaponPos.transform.localRotation = thirdPersonCamera.transform.localRotation;
            interactDis = 5;
        }
    }

    // Health and UI interactions
    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flahScreen());
        if (HP <= 0)
        {
            GameManager.instance.youLose();
        }

    }
    IEnumerator flahScreen()
    {
        GameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageFlash.SetActive(false);
    }
    public void updatePlayerUI()
    {
        GameManager.instance.PlayerHP_bar.fillAmount = (float)HP / HPMax;
    }

    // Item Interactions
    public void pickUpObject(Pickups item)
    {

        if (itemList.Contains(item))
        {
            itemIndex = itemList.IndexOf(item);
            itemList[itemIndex].uesage++;

        }
        else
        {
            itemList.Add(item);
            itemIndex = itemList.Count - 1;
            itemList[itemIndex].uesage = 1;
        }
        if (activePick == null)
        {
            changeItem(itemIndex);
        }
    }
    void changeItem(int pos)
    {
        activePick = itemList[pos];
        itemIndex = itemList[pos].itemIndex;
        GameManager.instance.updateItem(itemIndex);
    }
    void useItem()
    {
        if (activePick != null && Input.GetButtonDown("Use"))
        {
            //Healing if used object has health
            if (activePick.healing > 0)
            {
                Heal(activePick.healing);
                activePick.uesage--;
            }
            //add to max ammo
            if (activePick.ammo > 0)
            {
                Shooting.instance.maxAmmo += activePick.ammo;
                Shooting.instance.callAmmo();
                activePick.uesage--;
            }
            //temp Boost dmg
            if (activePick.dmgBoost > 0)
            {
                if(dmgBoosting == false)
                {
                    activePick.uesage--;
                    StartCoroutine(dmgBoost());
                }
            }
            //temp speed Boost
            if (activePick.speedBoost > 0)
            {
                if(speed == DataManager.instance.currentRunStats[2])
                {
                    activePick.uesage--;
                    StartCoroutine(speedBoost());
                }
            }
            //Check for usage and remove if no more uses
            if (activePick.uesage <= 0)
            {
                itemList.Remove(activePick);
                if (itemList.Count > 0)
                {
                    activePick = itemList[itemList.Count - 1];
                    itemIndex = itemList[itemList.Count - 1].itemIndex;
                    GameManager.instance.updateItem(itemIndex);
                }
                else
                {
                    activePick = null;
                    GameManager.instance.updateItem(0);
                }
            }
        }
    }
    public void Heal(int amount)
    {
        HPMax += amount;
        if (HP > HPMax)
        {
            HP = HPMax;
        }
        updatePlayerUI();
    }
    IEnumerator dmgBoost() 
    {
        dmgBoosting = true;
        tempOrginDmg = Shooting.instance.gunList[gunPos].bullet.damageAmount;
        Shooting.instance.gunList[gunPos].bullet.damageAmount *= (int)activePick.dmgBoost;
        boostTime = activePick.boostDur;
        yield return new WaitForSeconds(boostTime);
        Shooting.instance.gunList[gunPos].bullet.damageAmount = tempOrginDmg;
        dmgBoosting = false;
    }
    IEnumerator speedBoost()
    {
        tempOrginSpeed = speed;
        boostTime = activePick.boostDur;
        speed *= (int)activePick.speedBoost;
        yield return new WaitForSeconds((float)boostTime);
        speed = tempOrginSpeed;
    }

    // World Interactions
    public void spawnPlayer()
    {
        playerController.transform.position = GameManager.instance.playerSpawn.transform.position;
        Physics.SyncTransforms();
        HP = HPMax;
        updatePlayerUI();
    }
    public void playAudio(AudioClip clip, float volume)
    {
        aud.PlayOneShot(clip, volume);
    }

    // Data Management
    public void setBaseStats()
    {
        DataManager.instance.basePlayerStats.Add(HPMax);
        DataManager.instance.basePlayerStats.Add(speed);
        DataManager.instance.basePlayerStats.Add(jumpMax);
        DataManager.instance.basePlayerStats.Add(jumpSpeed);
        updateStats();
    }
    public void updateStats()
    {
        if (DataManager.instance.currentRunStats.Count < 1)
        {
            DataManager.instance.currentRunStats.Add(SceneManager.GetActiveScene().buildIndex);
            DataManager.instance.currentRunStats.Add(HPMax);
            DataManager.instance.currentRunStats.Add(speed);
            DataManager.instance.currentRunStats.Add(jumpMax);
            DataManager.instance.currentRunStats.Add(jumpSpeed);
        }
        else
        {
            DataManager.instance.currentRunStats[0] = SceneManager.GetActiveScene().buildIndex;
            DataManager.instance.currentRunStats[1] = HPMax;
            DataManager.instance.currentRunStats[2] = speed;
            DataManager.instance.currentRunStats[3] = jumpMax;
            DataManager.instance.currentRunStats[4] = jumpSpeed;
        }
    }
    public void getRunStats()
    {
        HPMax = DataManager.instance.currentRunStats[1];
        speed = DataManager.instance.currentRunStats[2];
        jumpMax = DataManager.instance.currentRunStats[3];
        jumpSpeed = DataManager.instance.currentRunStats[4];
        Shooting.instance.gunList.Clear();
        foreach (var gun in DataManager.instance.currentGuns)
        {
            Shooting.instance.gunList.Add(gun);
        }
        Shooting.instance.changeGun(gunPos);
    }
}
