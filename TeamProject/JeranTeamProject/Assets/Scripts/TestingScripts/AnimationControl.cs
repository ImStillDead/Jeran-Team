using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator animator;
    
    //Animation Settings
    public bool isRunning;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.playerScript.playerAnimator = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
}
