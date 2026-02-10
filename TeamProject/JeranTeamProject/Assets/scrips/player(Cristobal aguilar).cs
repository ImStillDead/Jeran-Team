using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class playerController : MonoBehaviour
{

    [SerializeField] CharacterController controller;

    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] int currentAmmo;
    [SerializeField] int max_Ammo;
    

    int jumpCount;
    int HPorg;

    Vector3 moveDir;
    Vector3 playerVel;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();



    }

    void Movement()
    {


        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        jump();
        controller.Move(playerVel * Time.deltaTime);

        playerVel.y -= gravity * Time.deltaTime;
        sprint();

    }
    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
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
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    public void updatePlayerUI()
    {

        GameManager.instance.update_ammocount(currentAmmo, max_Ammo);


    }

}
