using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController playerController;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int interactDis;
    [SerializeField] int gravity;
    [SerializeField] Transform weaponPos;
    [SerializeField] GameObject firstPersonCamera;
    [SerializeField] GameObject thirdPersonCamera;
    [SerializeField] GameObject inventory1;
    [SerializeField] GameObject inventory2;
    [SerializeField] GameObject inventory3;

    GameObject activeItem;
    int HPOrigin;
    int jumpCount;
    bool isFirstPerson;
    Vector3 moveDir;
    Vector3 playerVel;

    void Start()
    {
        HPOrigin = HP;
        activeItem = Instantiate(inventory1, weaponPos);
        isFirstPerson = true;
        updatePlayerUI();
    }

    void Update()
    {
        Movement();
        WeaponRotate();
    }
    void Movement()
    {
       
        //directional input
        moveDir = Input.GetAxis("Horizontal") * transform.right + (Input.GetAxis("Vertical") * transform.forward);
        playerController.Move(moveDir * speed * Time.deltaTime);
        //jump method
        Jump();
        playerController.Move(playerVel * Time.deltaTime);
        //gravity
        playerVel.y -= gravity * Time.deltaTime;
        //jumpCheck
        if (playerController.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }
        CameraToggle();
        if (Input.GetButtonDown("Interact"))
        {
            Interact();
        }
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
            if (interact.collider.gameObject.CompareTag("Door") == true)
            {
                if (interact.collider.gameObject.GetComponent<Collider>().isTrigger == false)
                {
                    interact.collider.gameObject.GetComponent<Collider>().isTrigger = true;
                    interact.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                else if (interact.collider.isTrigger)
                {
                    interact.collider.gameObject.GetComponent<Collider>().isTrigger = false;
                    interact.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashScreen());

        if (HP <= 0)
        {
            GameManager.instance.youLose();
        }

    }

    IEnumerator flashScreen()
    {
        GameManager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageFlash.SetActive(false);
    }

    public void updatePlayerUI()
    {
        GameManager.instance.PlayerHP_bar.fillAmount = (float)HP / HPOrigin;
    }

}
