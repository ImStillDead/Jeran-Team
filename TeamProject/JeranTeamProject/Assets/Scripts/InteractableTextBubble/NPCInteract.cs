using UnityEngine;
using TMPro;

public class NPCInteract : MonoBehaviour, iInteract
{
    [SerializeField] private GameObject bubbleObject;
    [SerializeField] private TMP_Text bubbleText;

    [TextArea(2, 5)]
    [SerializeField] private string message = "Hello there.";

    [SerializeField] private bool useWorldBubble = true;
    [SerializeField] private bool useGameManagerDialog = true;

    private bool bubbleShowing = false;


    void Start()
    {
        if (bubbleObject != null)
        {
            bubbleObject.SetActive(false);
        }
        if (bubbleText != null)
        { 
            bubbleText.text = message;
        }
    }

    
    public void Interacted()
    {
        Debug.Log("this charter has been interacted with");
        if (useGameManagerDialog && GameManager.instance != null)
        {
            GameManager.instance.addDialog(message);
        }
        if (useWorldBubble && bubbleObject != null)
        {
            bubbleShowing = !bubbleShowing;
            bubbleObject.SetActive(bubbleShowing);

            if(bubbleText != null)
            {
                bubbleText.text = message;
            }
        }
    }

    public void StopInteraction()
    {
        if (bubbleObject != null)
        {
            bubbleShowing = false;
            bubbleObject.SetActive(false);
        }
    }
    // Im trying this out not sure if it will work, but I want to try to make it so that when the player interacts with the NPC, it will show the bubble for a certain amount of time and then hide it again.
}
