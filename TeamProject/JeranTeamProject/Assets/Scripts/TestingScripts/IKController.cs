using UnityEngine;


[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{
    public IKController ikController;
    public Animator animator;

    public Transform gunRightHand;
    public Transform gunLeftHand;
    public Transform scope;
    //public Transform rightHand;
    private void Start()
    {

    }
    private void OnAnimatorIK()
    {
        if (scope != null)
        {
            animator.SetLookAtPosition(scope.position);
            animator.SetLookAtWeight(1);
        }
        //if (gunRightHand != null)
        //{
        //    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        //    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        //    animator.SetIKPosition(AvatarIKGoal.RightHand, gunRightHand.position);
        //    animator.SetIKRotation(AvatarIKGoal.RightHand, gunRightHand.rotation);
            
        //}
        //if(gunLeftHand != null)
        //{
          
        //    if(Shooting.instance.GetGunType() == GunType.Pistol)
        //    {
        //        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, .7f);
        //        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, .5f);
        //        animator.SetIKPosition(AvatarIKGoal.LeftHand, gunLeftHand.position);
        //        animator.SetIKRotation(AvatarIKGoal.LeftHand, gunLeftHand.rotation);
        //    }
        //}
    }

}
