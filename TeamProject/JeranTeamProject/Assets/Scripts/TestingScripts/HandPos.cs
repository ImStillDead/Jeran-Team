using UnityEngine;

public class HandPos : MonoBehaviour
{
    PlayerController playerController;
    private void Start()
    {
        playerController = GameManager.instance.playerScript;
        if (this.CompareTag("RightHand"))
        {
            playerController.rightHand = transform;
        }
        else
        {
            playerController.leftHand = transform;
        }
    }
   
}
