using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamage, IPickup, IGunPickup
{
    public PlayerController instancePlayer;

    [SerializeField] CharacterController playerController;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int interactDis;
    [SerializeField] int enemyViewDis;
    [SerializeField] int gravity;
    [SerializeField] Transform weaponPos;
    [SerializeField] GameObject firstPersonCamera;
    [SerializeField] GameObject thirdPersonCamera;
    [SerializeField] GameObject torch;
    [SerializeField] List<GunStats> gunList = new List<GunStats>();
    [SerializeField] List<Pickups> itemList = new List<Pickups>();
    [SerializeField] AudioSource aud;
    Pickups activePick;
    int HPOrigin;
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
    void Start()
    {
        HPOrigin = HP;
        spawnPlayer();
        isFirstPerson = true;
        updatePlayerUI();
        invPos = 0;
        torch.SetActive(true);
        torchActive = true;
    }

    void Update()
    {
        Movement();
        WeaponRotate();
        Sprint();
    }
    void Movement()
    {
        moveDir = Input.GetAxis("Horizontal") * transform.right + (Input.GetAxis("Vertical") * transform.forward);
        playerController.Move(moveDir * speed * Time.deltaTime);
        Jump();
        SwitchWeapon();
        playerController.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;
        if (playerController.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }
        if (Input.GetButtonDown("ToggleCamera"))
        {
            CameraToggle();
        }
        if (Input.GetButtonDown("Interact"))
        {
            Interact();
        }
        if (activePick != null && Input.GetButtonDown("Use"))
        {
            useItem();
        }
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
    void CameraToggle()
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
    void Jump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }
    }
    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        } else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }
    public void spawnPlayer()
    {
        playerController.transform.position = GameManager.instance.playerSpawn.transform.position;
        Physics.SyncTransforms();
        HP = HPOrigin;
        updatePlayerUI();
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
        if(activePick == null)
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
        activePick.uesage--;
        
        //Healing if used object has health
        if (activePick.healing > 0)
        {
            Heal(activePick.healing);
        }
        //add to max ammo
        if(activePick.ammo > 0)
        {
            Shooting.instance.maxAmmo += activePick.ammo;
            Shooting.instance.callAmmo();
        }
        if(activePick.dmgBoost > 0)
        {
            StartCoroutine(dmgBoost());
        }
        if(activePick.speedBoost > 0)
        {
            StartCoroutine(speedBoost());
        }
        //Check for usage and remove if no more uses
        if (activePick.uesage <= 0)
        {
            itemList.Remove(activePick);
            if(itemList.Count > 0)
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
   
    void SwitchWeapon()
    {

        if(Input.GetButtonDown("Swap"))
        {
            if(invPos >= itemList.Count - 1)
            {
                invPos = 0;
            }
            else
            {
                invPos++;
            }
              
            changeItem(invPos);            
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(gunPos >= gunList.Count - 1)
            {
                gunPos = 0;
            }
            else
            {
                gunPos++;
            }

            Shooting.instance.changeGun(gunList[gunPos]);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (gunPos <= 0)
            {
                gunPos = gunList.Count - 1;
            }
            else
            {
                gunPos--;
            }

            Shooting.instance.changeGun(gunList[gunPos]);
        }


        //if (Input.GetAxis("Mouse ScrollWheel") > 0 && invPos < itemPickup.Count - 1)
        //{
        //    invPos++;
        //    changeItem(invPos);
        //    itemIndex = activePick.itemIndex;
        //    GameManager.instance.updateItem(itemIndex);
        //}
        //else if (Input.GetAxis("Mouse ScrollWheel") < 0 && invPos > 0)
        //{
        //    invPos--;
        //    changeItem(invPos);
        //    itemIndex = activePick.itemIndex;
        //    GameManager.instance.updateItem(itemIndex);
        //}
        
        if (Input.GetButtonDown("Weapon1"))
        {
            gunPos = 0;
            Shooting.instance.changeGun(gunList[gunPos]);
        }
        else if (Input.GetButtonDown("Weapon2"))
        {
            gunPos = 1;
            Shooting.instance.changeGun(gunList[gunPos]);
        }
        else if (Input.GetButtonDown("Weapon3"))
        {
            gunPos = 2;
            Shooting.instance.changeGun(gunList[gunPos]);
        }
        else if (Input.GetButtonDown("Weapon4"))
        {
            gunPos = 3;
            Shooting.instance.changeGun(gunList[gunPos]);
        }
        else if (Input.GetButtonDown("Weapon5"))
        {
            gunPos = 4;
            Shooting.instance.changeGun(gunList[gunPos]);
        }
    }
    void WeaponRotate()
    {
        if (isFirstPerson)
        {
            weaponPos.transform.localRotation = firstPersonCamera.transform.localRotation;
            interactDis = 3;
        }
        else
        {
            weaponPos.transform.localRotation = thirdPersonCamera.transform.localRotation;
            interactDis = 5;
        }
    }
    void Interact()
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
        GameManager.instance.PlayerHP_bar.fillAmount = (float)HP / HPOrigin;
    }

    public void Heal(int amount)
    {
        HP += amount;
        if (HP > HPOrigin)
        {
            HP = HPOrigin;
        }
        updatePlayerUI();
    }
    IEnumerator dmgBoost() 
    {
        tempOrginDmg = gunList[gunPos].bullet.damageAmount;
        gunList[gunPos].bullet.damageAmount *= (int)activePick.dmgBoost;
        boostTime = activePick.boostDur;
        dmgBoosting = true;
        yield return new WaitForSeconds(boostTime);
        gunList[gunPos].bullet.damageAmount = tempOrginDmg;
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

    public void GetGunStats(GunStats gun)
    {
        //gun.rotation = weaponPos.localRotation;
        //gun.postion = weaponPos.transform.position;
        gunList.Add(gun);
        gunPos = gunList.Count - 1;
        if(gunList.Count == 1)
        {
            Shooting.instance.changeGun(gunList[gunPos]);
        }
    }
    public void playAudio(AudioClip clip, float volume)
    {
        aud.PlayOneShot(clip, volume);
    }
    public void updateGunAmmo()
    {
        gunList[gunPos].currentAmmo = Shooting.instance.currentAmmo;
        gunList[gunPos].maxAmmo = Shooting.instance.maxAmmo;
    }
}
