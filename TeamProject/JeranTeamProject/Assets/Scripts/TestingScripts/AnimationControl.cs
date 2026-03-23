using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator animator;
    private GameObject player;
    private PlayerController playerController;
    //Animation Settings
    float magnitudeX;
    float magnitudeZ;
    float reach;
    Transform gunPos;
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
        //MoveAnimations
        magnitudeX = (Input.GetAxis("Horizontal") * player.transform.right).magnitude * playerController.GetSpeed();
        magnitudeZ = (Input.GetAxis("Vertical") * player.transform.forward).magnitude * playerController.GetSpeed();
        if(Input.GetAxis("Horizontal") < 0)
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
        if (Shooting.instance.GetGunType() == GunType.Pistol)
        {
            animator.SetLayerWeight(1, 1);
            //animator.SetLayerWeight(2, 0);
        } else if (Shooting.instance.GetGunType() != GunType.Pistol)
        {
            animator.SetLayerWeight(1, 0);
            // animator.SetLayerWeight(2, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
       
    }
}
