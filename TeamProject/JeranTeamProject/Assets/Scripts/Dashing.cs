using UnityEngine;
using System.Collections;

public class Dashing : MonoBehaviour , IDash
{
    [HideInInspector] public float dashForce = 30f;
    [HideInInspector] public float dashDur = 0.2f;
    [HideInInspector] public float dashCD = 1f; //CD = cooldown

    private CharacterController Controller;
    private PlayerController playerController;
    private float dashCDTimer;
    private bool isDashing;
    private Vector3 dashDir;

    private int originalSpeed;
    private int originalJumpSpeed;


    void Start()
    {
        Controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (dashCDTimer > 0)
            dashCDTimer -= Time.deltaTime;

        if(Input.GetButtonDown("Dash")&& !isDashing && dashCDTimer <= 0)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                dashDir = (transform.right * horizontal + transform.forward * vertical).normalized;
            }
            else
            {
                dashDir = transform.forward;
            }
            StartDash();
        }
    }

    public void StartDash()
    {
        isDashing = true;
        dashCDTimer = dashCD;

        originalSpeed = playerController.GetSpeed();
        originalJumpSpeed = playerController.GetJumpSpeed();

        playerController.SetSpeed(0);
        playerController.SetJumpSpeed(0);

        StartCoroutine(PerformDash());
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    public float GetDashRemainingCooldown()
    {
        return dashCDTimer;
    }

    IEnumerator PerformDash()
    {
        float elapsedTime = 0f;

        while (elapsedTime < dashDur)
        {
            Controller.Move(dashDir * dashForce * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerController.SetSpeed(originalSpeed);
        playerController.SetJumpSpeed(originalJumpSpeed);
        isDashing = false;
    }

    
}
