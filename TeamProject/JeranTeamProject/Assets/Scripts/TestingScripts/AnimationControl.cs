using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator animator;
    private GameObject player;
    private PlayerController playerController;
    //Animation Settings
    public bool isJumping;
    public bool isAiming;
    public bool isReloading;
    float magnitudeX;
    float magnitudeZ;
    float reach;
    public Transform leftHand;
    float movingMag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.instance.player;
        playerController = GameManager.instance.playerScript;
        playerController.playerAnimator = this;

    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
        {
            //MoveAnimations
            magnitudeX = (Input.GetAxis("Horizontal") * player.transform.right).magnitude * playerController.GetSpeed();
            magnitudeZ = (Input.GetAxis("Vertical") * player.transform.forward).magnitude * playerController.GetSpeed();
            if (Input.GetAxis("Horizontal") < 0)
            {
                magnitudeX = -magnitudeX;
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                magnitudeZ = -magnitudeZ;
            }
            animator.SetFloat("VelocityX", magnitudeX);
            animator.SetFloat("VelocityZ", magnitudeZ);
            //GunCheckAnimations 
            if (Shooting.instance.gunList.Count > 0)
            {
                if (Shooting.instance.GetGunType() == GunType.Pistol)
                {
                    animator.SetLayerWeight(1, 1);
                    animator.SetLayerWeight(2, 0);
                }
                else if (Shooting.instance.GetGunType() != GunType.Pistol)
                {
                    animator.SetLayerWeight(1, 0);
                    animator.SetLayerWeight(2, 1);
                }
                else
                {
                    animator.SetLayerWeight(1, 0);
                }
            }
            animator.SetBool("isJump", isJumping);
            if (isJumping)
            {
                movingMag = (Mathf.Abs(magnitudeX) + Mathf.Abs(magnitudeZ)) / 2;
                animator.SetFloat("JumpSpeed", movingMag);
            }
            if (Input.GetButton("Fire1"))
            {
                animator.SetBool("Fire", true);
            }
            else
            {
                animator.SetBool("Fire", false);
            }
            animator.SetBool("Aim", isAiming);
            animator.SetBool("reload", isReloading);
        }
    }
 


}

