using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamage
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
    [SerializeField] List<GameObject> inventoryList = new List<GameObject>();
    

    GameObject activeItem;
    int HPOrigin;
    int jumpCount;
    int invPos;
    bool isFirstPerson;
    Vector3 moveDir;
    Vector3 playerVel;
    int sceneIndex;
    void Start()
    {
        HPOrigin = HP;
        isFirstPerson = true;
        updatePlayerUI();
        invPos = 0;
        swapWeapon();
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
    void swapWeapon()
    {
        Destroy(activeItem);
        activeItem = Instantiate(inventoryList[invPos], weaponPos);
    }
    void SwitchWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && invPos < inventoryList.Count - 1)
        {
            invPos++;
            swapWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && invPos > 0)
        {
            invPos--;
            swapWeapon();
        }
            if (Input.GetButtonDown("Weapon1"))
        {
            invPos = 0;
            swapWeapon();
        }
        else if (Input.GetButtonDown("Weapon2"))
        {
            invPos = 1;
            swapWeapon();
        }
        else if (Input.GetButtonDown("Weapon3"))
        {
            invPos = 2;
            swapWeapon();
        }
    }
    void WeaponRotate()
    {
        if (isFirstPerson)
        {
            activeItem.transform.localRotation = firstPersonCamera.transform.localRotation;
            interactDis = 3;
        }
        else
        {
            activeItem.transform.localRotation = thirdPersonCamera.transform.localRotation;
            interactDis = 5;
        }
    }
    void Interact()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactDis, Color.red);
        RaycastHit interact;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out interact, interactDis, ~ignoreLayer))
        {
            
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
}
