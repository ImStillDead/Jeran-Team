using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController playerController;
    [SerializeField] LayerMask ignore;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;
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
        activeItem = inventory1;
        isFirstPerson = true;
    }

    void Update()
    {
        movement();
    }
    void movement()
    {
       
        //directional input
        moveDir = Input.GetAxis("Horizontal") * transform.right + (Input.GetAxis("Vertical") * transform.forward);
        playerController.Move(moveDir * speed * Time.deltaTime);
        //jump method
        jump();
        playerController.Move(playerVel * Time.deltaTime);
        //gravity
        playerVel.y -= gravity * Time.deltaTime;
        //jumpCheck
        if (playerController.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }
        cameraToggle();

    }
    void cameraToggle()
    {
        if (Input.GetButtonDown("ToggleCamera"))
        {
            if(isFirstPerson == true)
            {
                thirdPersonCamera.SetActive(true);
                firstPersonCamera.SetActive(false);
                isFirstPerson = false;
            }
            else
            {
                firstPersonCamera.SetActive(true);
                thirdPersonCamera.SetActive(false);
                isFirstPerson = true;
            }
        }
    }
    void cameraTP()
    {
        if (Input.GetButtonDown("ToggleCamera"))
        {
            isFirstPerson = false;
            thirdPersonCamera.SetActive(true);
            firstPersonCamera.SetActive(false);
        }
    }
    void jump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }
    }
    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        } else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }
    public void takeDamage(int ammount)
    {
        HP -= ammount; 
    }
}
