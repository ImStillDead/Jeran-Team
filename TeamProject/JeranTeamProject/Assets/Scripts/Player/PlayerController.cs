using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;


public class PlayerController : MonoBehaviour, IDamage, IPickup, IGunPickup, IDash, ICharacters
{
    [SerializeField] CharacterController playerController;
    [SerializeField] LayerMask ignoreLayer;

    [Header("Player Stats")]
    public int level;
    public float experience;
    public float levelUpCap;
    [SerializeField] float HP;
    [SerializeField] float HPMax;
    [SerializeField] int Armor;
    [SerializeField] int maxArmor;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpChargeMax;
    [SerializeField] float jumpChargeRate;
    [SerializeField] int jumpMax;
    [SerializeField] float armorRegenDelay;
    [SerializeField] float armorRegenRate;
    [SerializeField] int gunMax;


    [Header("Player Static Stats")]
    [SerializeField] int interactDis;
    [SerializeField] int enemyViewDis;
    [SerializeField] int gravity;
    [SerializeField] int hubIndex;
    [SerializeField] GameObject lookAt;


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
    [SerializeField] int moneyOnPlayer;
    [SerializeField] public Transform weaponPos;
    [SerializeField] GameObject firstPersonCamera;
    [SerializeField] GameObject torch;
    [SerializeField] AudioSource aud;
    [SerializeField] List<GameObject> MeshList;

    private cardHolder cardUI;
    GameManager manager;
    public GameObject characterMesh;
    PlayerData staticBase;
    public Shooting Gun;
    GameData runData;
    [SerializeField] public IKController playerIK;
    public AnimationControl playerAnimator;
    public PlayerData playerData;
    public List<Pickups> itemList;
    Pickups activePick;
    float lastDamageTime;
    float armorRegenTimer;
    int jumpCount;
    int invPos;
    int gunPos;
    int itemIndex;
    float boostTime;
    float jumpCharge;
    bool dmgBoosting;
    public bool isJumping;
    bool torchActive;
    public bool canPickup;
    public bool canSwap;
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
    private Vector3 slideDirection;
    private CharacterController characterController;
    private bool slideButtonHeld;
    private Vector3 platformVelocity;

    private Transform currentPlatform;
    private Vector3 lastPlatformPos;

    public List<SetBoneContraint> boneRigs = new List<SetBoneContraint>();
    public Transform rightHand;
    public Transform leftHand;

    void Awake()
    {
        runData = new GameData();
        staticBase = new PlayerData();
        playerData = new PlayerData();
        characterController = GetComponent<CharacterController>();
        SetupDashing();
        SetupSliding();
    }
    void Start()
    {
        Gun = Shooting.instance;
        manager = GameManager.instance;
        SpawnPlayer();
        PlayerArmor();
        UpdatePlayerUI();
        cardUI = FindAnyObjectByType<cardHolder>();
        cardUI?.Init(this);
    }
    void Update()
    {
        if (manager != null && manager.menus.isPaused)
            return;
        

        UpdatePlayerUI();
        Movement();
        Sprint();
        WeaponRotate();
        ArmorRegen();
        Charge();
    }
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
            playerVel.y -= gravity * Time.deltaTime;
            playerController.Move(playerVel * Time.deltaTime);
            return;
        }


        if (currentPlatform != null)
        {
    

            lastPlatformPos.y = manager.player.transform.position.y;
            platformVelocity = currentPlatform.position - lastPlatformPos;

            playerController.Move(platformVelocity);
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + (Input.GetAxis("Vertical") * transform.forward);
        playerController.Move(speed * Time.deltaTime * moveDir);
        playerController.Move(playerVel * Time.deltaTime);

        playerVel.y -= gravity * Time.deltaTime;



        if (playerController.isGrounded && playerVel.y < 0)
        {
            playerVel.y = -2f;
            jumpCount = 0;
            playerAnimator.isJumping = false;
        }

        Jump();
        ChangeActiveInventory();
        Interact();
        UseItem();
        ToggleTorch();

    }
    //Dashing and Sliding
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
        originalHeight = characterController.height;
        originalCenter = characterController.center;
        originalYScale = transform.localScale.y;
    }
    void HandleSliding()
    {
        if (slideCDTimer > 0)
            slideCDTimer -= Time.deltaTime;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool isMoving = (horizontalInput != 0 || verticalInput != 0);

        if (isMoving)
        {
            slideDirection = (transform.right * horizontalInput + transform.forward * verticalInput).normalized;
        }

        if (Input.GetButtonDown("Slide") && canSlide && !isSliding && isMoving && slideCDTimer <= 0 && !IsDashing())
        {
            StartSlide();
        }

        if (Input.GetButtonUp("Slide") && isSliding)
        {
            StopSlide();
        }

        if (isSliding)
        {
            SlidingMovement();
        }
    }
    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDur;
        slideButtonHeld = true;

        characterController.height = originalHeight * slideYScale;

        Vector3 newCenter = originalCenter;
        newCenter.y = originalCenter.y * slideYScale;
        characterController.center = newCenter;

        transform.localScale = new Vector3(transform.localScale.x, originalYScale * slideYScale, transform.localScale.z);
    }
    void SlidingMovement()
    {
        playerController.Move(slideSpeed * Time.deltaTime * slideDirection);

        playerVel.y -= gravity * Time.deltaTime;
        playerController.Move(playerVel * Time.deltaTime);

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
        slideButtonHeld = false;

        characterController.height = originalHeight;
        characterController.center = originalCenter;

        transform.localScale = new Vector3(transform.localScale.x, originalYScale, transform.localScale.z);

        if (playerController.isGrounded)
        {
            playerVel.y = -2f;
        }
    }
    public bool IsSliding()
    {
        return isSliding;
    }
    public float GetSpeed()
    {
        return speed;
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public float GetJumpSpeed()
    {
        return jumpSpeed;
    }
    public void SetJumpSpeed(float newJumpSpeed)
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
    public Vector3 GetVel()
    {
        return playerVel;
    }
    //Jump
    void Jump()
    {
        if (isSliding)
            return;

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            isJumping = true;
        }
        if (Input.GetButtonUp("Jump") && jumpCount < jumpMax)
        {
            if(playerAnimator.isJumping == true)
            {
                playerAnimator.isJumping = false;
            }
            jumpCharge *= jumpChargeRate;
            isJumping = false;
            playerVel.y = jumpSpeed + jumpCharge;
            jumpCount++;
            jumpCharge = 0;
            playerAnimator.isJumping = true;
        }
    }
    void Charge()
    {
        if (jumpCharge < jumpChargeMax && isJumping)
        {
            jumpCharge += Time.deltaTime;
        }
    }
    //Sprint
    void Sprint()
    {
        if (IsDashing() || isSliding)
            return;

        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }
    //Torch
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
            ChangeItem(itemIndex);
        }
    }
    //Itemes and Interactions
    void ChangeItem(int pos)
    {
        activePick = itemList[pos];
        itemIndex = itemList[pos].itemIndex;
        GameManager.instance.updateItem(itemIndex);
    }
    void UseItem()
    {
        if (activePick != null && Input.GetButtonDown("Use"))
        {
            //Healing if used object has health
            if (activePick.healing > 0)
            {
                Heal(activePick.healing);
                activePick.uesage--;
            }
            //temp Boost dmg
            if (activePick.dmgBoost > 0)
            {
                if (dmgBoosting == false)
                {
                    activePick.uesage--;
                    StartCoroutine(DmgBoost());
                }
            }
            //temp speed Boost
            if (activePick.speedBoost > 0)
            {
                if (speed != playerData.speed)
                {
                    activePick.uesage--;
                    StartCoroutine(SpeedBoost());
                }
            }
            //Check for usage and remove if no more uses
            if (activePick.uesage <= 0)
            {
                itemList.Remove(activePick);
                if (itemList.Count > 0)
                {
                    activePick = itemList[^1];
                    itemIndex = itemList[^1].itemIndex;
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
        Armor = maxArmor;
        HP += amount;

        if (HP > HPMax)
            HP = (int)HPMax;
        playerData.HP = HP;
        UpdatePlayerUI();
    }
    IEnumerator DmgBoost()
    {
        dmgBoosting = true;
        playerData.dmgAmmount = Gun.gunList[gunPos].bullet.damageAmount;
        Gun.gunList[gunPos].bullet.damageAmount *= (int)activePick.dmgBoost;
        boostTime = activePick.boostDur;
        yield return new WaitForSeconds(boostTime);
        Gun.gunList[gunPos].bullet.damageAmount = playerData.dmgAmmount;
        dmgBoosting = false;
    }
    IEnumerator SpeedBoost()
    {
        boostTime = activePick.boostDur;
        speed *= (int)activePick.speedBoost;
        yield return new WaitForSeconds((float)boostTime);
        speed = playerData.speed;
    }
    void ChangeActiveInventory()
    {

        // Item Swap
        if (Input.GetButtonDown("Swap"))
        {
            if (itemList != null && itemList.Count > 0)
            {
                if (invPos >= itemList.Count - 1)
                    invPos = 0;
                else
                    invPos++;

                ChangeItem(invPos);
            }
        }

        if (Gun == null || Gun.gunList == null || Gun.gunList.Count == 0)
            return;


        // Weapon Scroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (gunPos >= Gun.gunList.Count - 1)
                gunPos = 0;
            else
                gunPos++;

            UpdateGun();
            UpdateAnimations();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (gunPos <= 0)
                gunPos = Gun.gunList.Count - 1;
            else
                gunPos--;

            UpdateGun();
            UpdateAnimations();
        }

        // Weapon Select 1-2
        if (Input.GetButtonDown("Weapon1") && Gun.gunList.Count > 0)
        {
            gunPos = 0;
            UpdateGun();
 
        }
        else if (Input.GetButtonDown("Weapon2") && Gun.gunList.Count > 1)
        {
            gunPos = 1;
            UpdateGun();

        }

    }
    void Interact()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Vector3 origin = Camera.main.transform.position;
            Vector3 direction = Camera.main.transform.forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hitInter, interactDis))
            {
                if (hitInter.collider.TryGetComponent<iInteract>(out var interactable))
                {
                    interactable.Interacted();
                }
            }
        }
    }
    // Gun interactions
    public void GetGunStats(GunStats gun)
    {
        if (Gun.gunList.Contains(gun))
        {
            canPickup = false;
        }
        else if (Gun.gunList.Count >= gunMax)
        {
            canPickup = false;
        }
        else
        {
            canPickup = true;
            Gun.gunList.Add(gun);
            gunPos = Gun.gunList.Count - 1;
            if (Gun.gunList.Count == 1)
            {
                Gun.changeGun(gunPos);
            }
        }

        cardUI?.updateCards();

    }
    public void SwapGunPickup(GunStats gun)
    {
        Gun.gunList[gunPos] = gun;
        Gun.changeGun(gunPos);

        cardUI?.updateCards();

    }
    public void UpdateGun()
    {
        Gun.changeGun(gunPos);

        cardUI?.updateCards();
    }
    void WeaponRotate()
    {
        weaponPos.transform.LookAt(lookAt.transform);
    }

    // Health and UI interactions
    public void takeDamage(int amount)
    {
        lastDamageTime = Time.time;
        armorRegenTimer = 0f;

        if (Armor > 0)
        {
            Armor--;
            manager.removeArmor();
        }
        else
        {
            playerData.HP -= amount;
        }

        UpdatePlayerUI();
        StartCoroutine(FlahScreen());

        if (playerData.HP <= 0)
        {
            GameManager.instance.menus.youLose();
        }


    }
    IEnumerator FlahScreen()
    {
        GameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageFlash.SetActive(false);
    }
    public void UpdatePlayerUI()
    {
        if (manager == null) return;

        float tartget = (float)playerData.HP / playerData.HPMax;
        float XPtarget = (float)playerData.experience / playerData.levelUpCap;

        if (manager.PlayerHP_bar != null)
        {
            manager.PlayerHP_bar.fillAmount = Mathf.Lerp(manager.PlayerHP_bar.fillAmount, tartget, Time.deltaTime * 50);

        }
        if (manager.XP_bar != null)
        {
            manager.XP_bar.fillAmount = Mathf.Lerp(manager.XP_bar.fillAmount, XPtarget, Time.deltaTime * 3);
            manager.levelText.text = playerData.level.ToString();
        }

        if (manager.moneyCount != null)
            playerData.money = moneyOnPlayer;

        if (manager.heathNum != null && manager.maxHealthNum != null)
        {
            manager.heathNum.text = Mathf.RoundToInt(playerData.HP).ToString();
            manager.maxHealthNum.text = Mathf.RoundToInt(playerData.HPMax).ToString();
        }
        if (itemList.Count > 0 && itemList[0] != null)
        {
            manager.updateItem(itemList[itemIndex].itemIndex);
        }
        manager.UpdateUI(playerData);
    }
    public void SpawnPlayer()
    {
        if (manager.playerSpawn != null)
        {
            playerController.transform.position = manager.playerSpawn.transform.position;
            Physics.SyncTransforms();
            UpdatePlayer(staticBase);
            if (manager.menus != null) manager.menus.stateUnpause();
        }
    }
    public void PlayAudio(AudioClip clip, float volume)
    {
        aud.PlayOneShot(clip, volume);
    }
    public void SetMaxHp(float input)
    {
        HPMax = input;
    }
    public float GetMaxHP()
    {
        return HPMax;
    }
    //Armor
    void PlayerArmor()
    {
        manager.addArmor(maxArmor);
        Armor = maxArmor;

    }
    void ArmorRegen()
    {
        // wait after taking damage
        if (Time.time < lastDamageTime + armorRegenDelay)
            return;

        // already full
        if (Armor >= maxArmor)
            return;

        // build up time
        armorRegenTimer += Time.deltaTime;

        if (armorRegenTimer >= armorRegenRate)
        {
            armorRegenTimer = 0f;

            Armor++;
            manager.addArmor(1); // ?? update UI
        }

    }
    //Money
    public void AddPlayerMoney(int increase)
    {
        moneyOnPlayer += increase;


    }
    public bool RemovePlayerMoney(int decrease)
    {
        if ((moneyOnPlayer - decrease) >= 0)
        {
            moneyOnPlayer -= decrease;
            return true;
        }
        else
        {
            manager.addDialog("you to broke to buy this item");
            return false;
        }
    }
    public int GetplayerMoney()
    {
        return moneyOnPlayer;
    }
    //SetData GetData
    public void UpdatePlayer(PlayerData data)
    {
        bool levelUp = false;
        if (level != data.level)
        {
            levelUp = true;
        }
        level = data.level;
        experience = data.experience;
        levelUpCap = data.levelUpCap;
        HPMax = data.HPMax;
        HP = data.HP;
        speed = data.speed;
        sprintMod = data.speedMod;
        jumpSpeed = data.jumpSpeed;
        jumpMax = data.jumpMax;
        jumpChargeRate = data.jumpChargeRate;
        jumpChargeMax = data.jumpChargeMax;
        itemList.Clear();
        foreach (Pickups item in data.itemList)
        {
            itemList.Add(item);
        }
        Gun.gunList.Clear();
        foreach (GunStats gun in data.gunList)
        {
            Gun.gunList.Add(gun);
        }
        if (!levelUp)
        {
            Gun.changeGun(0);
        }
        UpdatePlayerUI();
    }
    public void SetHubStats(GameData hubData)
    {
        if (DataManager.instance != null)
        {
            UpdatePlayer(hubData.playerData);
            //get achievements here

            DataManager.instance.SaveData(hubData);
        }
    }
    public PlayerData GetPlayerData()
    {
        return playerData;
    }
    //SaveData
    public void SaveHub()
    {
        if(DataManager.instance != null)
        {
            DataManager.instance.hubData.playerData = staticBase;
            DataManager.instance.hubData.sceneIndex = hubIndex;
        }
    }
    public void SaveRun() 
    {
        if (DataManager.instance != null)
        {
            runData.playerData = playerData;
            runData.currentpickUps = manager.pickUpObjects;
            runData.player = GameManager.instance.player;
            runData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            DataManager.instance.SaveRun(runData);
        }
    }
    //LoadData
    public void LoadRun()
    {
        if (DataManager.instance != null)
        {
            if (runData == null)
            {
                runData = new GameData();
            }
            runData = DataManager.instance.LoadRun();
            UpdatePlayer(runData.playerData);
        }
    }
    public void LoadSave()
    {
        if (DataManager.instance != null)
        {
            SetHubStats(DataManager.instance.LoadData());
        }
    }
    public void SwapCharacter(CharacterSelect character)
    {
        playerData.HPMax = character.HPMax;
        playerData.HP = character.HP;
        playerData.speed = character.speed;
        playerData.speedMod = character.speedMod;
        playerData.jumpSpeed = character.jumpSpeed;
        playerData.jumpMax = character.jumpMax;
        playerData.jumpChargeRate = character.jumpChargeRate;
        playerData.jumpChargeMax = character.jumpChargeMax;
        playerData.itemList.Clear();
        foreach (Pickups item in character.itemList)
        {
            playerData.itemList.Add(item);
        }
        playerData.gunList.Clear();
        foreach (GunStats gun in character.gunList)
        {
            playerData.gunList.Add(gun);
        }
        weaponPos.SetParent(GameManager.instance.player.transform);
        //MeshChange
        Destroy(characterMesh);
        characterMesh = Instantiate(character.mesh, this.transform);
        characterMesh.transform.localPosition = Vector3.zero;
        UpdateAnimations();
        UpdateStatic(playerData);
        UpdatePlayer(playerData);
    }
    void UpdateStatic(PlayerData character)
    {
        staticBase.HPMax = character.HPMax;
        staticBase.HP = character.HP;
        staticBase.speed = character.speed;
        staticBase.speedMod = character.speedMod;
        staticBase.jumpSpeed = character.jumpSpeed;
        staticBase.jumpMax = character.jumpMax;
        staticBase.jumpChargeRate = character.jumpChargeRate;
        staticBase.jumpChargeMax = character.jumpChargeMax;
        staticBase.itemList.Clear();
        foreach (Pickups item in character.itemList)
        {
            staticBase.itemList.Add(item);
        }
        staticBase.gunList.Clear();
        foreach (GunStats gun in character.gunList)
        {
            staticBase.gunList.Add(gun);
        }
        SaveHub();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            currentPlatform = other.transform;
            lastPlatformPos = currentPlatform.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == currentPlatform)
        {
            currentPlatform = null;
            
        }
    }

    public void UpdateAnimations()
    {
        playerAnimator.animator.avatar = characterMesh.GetComponent<Animator>().avatar;
        foreach(SetBoneContraint rig in boneRigs)
        {
            rig.updateRig();
        }
    }
}