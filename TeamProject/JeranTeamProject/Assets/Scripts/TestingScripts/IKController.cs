using UnityEngine;


[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{
    public IKController ikController;
    [SerializeField] public Animator animator;
    AnimationControl animationController;
    public Transform gunRightHand;
    public Transform gunLeftHand;
    public Transform scope;
    //public Transform rightHand;
    private void Start()
    {
        ikController = this;
    }
    private void OnAnimatorIK()
    {
        if(animationController == null)
        {
            animationController = GameManager.instance.playerScript.playerAnimator;
        }
        if (scope != null)
        {
            animator.SetLookAtPosition(scope.position);
            animator.SetLookAtWeight(1);
        }
        if (Shooting.instance.gunList[Shooting.instance.activeGun].canHold)
        {
            if (gunRightHand != null && !animationController.isReloading)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, .8f);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, gunRightHand.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, gunRightHand.rotation);
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
            }

            if (gunLeftHand != null && !animationController.isReloading)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, gunLeftHand.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, gunLeftHand.rotation);
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
            }
        }
    }

}
