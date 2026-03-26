using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetBoneContraint : MonoBehaviour
{
    Animator characterAnimator;
    TwoBoneIKConstraint iKConstraint;
    private void Start()
    {
        iKConstraint = this.GetComponent<TwoBoneIKConstraint>();
        GameManager.instance.playerScript.boneRigs.Add(this);
    }
    public void SetBoneRight()
    {
        characterAnimator = GameManager.instance.playerScript.playerIK.animator;
        iKConstraint.data.root = characterAnimator.GetBoneTransform(HumanBodyBones.RightShoulder);
        iKConstraint.data.mid = characterAnimator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        iKConstraint.data.tip = characterAnimator.GetBoneTransform(HumanBodyBones.RightHand);
    }
    public void SetBoneLeft()
    {
        characterAnimator = GameManager.instance.playerScript.playerIK.animator;
        iKConstraint.data.root = characterAnimator.GetBoneTransform(HumanBodyBones.LeftShoulder);
        iKConstraint.data.mid = characterAnimator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        iKConstraint.data.tip = characterAnimator.GetBoneTransform(HumanBodyBones.LeftHand);
    }
    public void SetBoneChest()
    {
        characterAnimator = GameManager.instance.playerScript.playerIK.animator;
        iKConstraint.data.root = characterAnimator.GetBoneTransform(HumanBodyBones.Hips);
        iKConstraint.data.mid = characterAnimator.GetBoneTransform(HumanBodyBones.Spine);
        iKConstraint.data.tip = characterAnimator.GetBoneTransform(HumanBodyBones.Chest);
    }
    public void updateRig()
    {
        if(gameObject.name == "RightArmConstraints")
        {
            SetBoneRight();
        }
        if (gameObject.name == "LeftArmConstraints")
        {
            SetBoneLeft();
        }
        if (gameObject.name == "UpperBody")
        {
            SetBoneChest();
        }
    }
}
