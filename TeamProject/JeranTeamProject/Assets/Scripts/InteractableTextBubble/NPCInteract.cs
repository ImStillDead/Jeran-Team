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

    GameManager manager;


    void Start()
    {

        if (bubbleText != null)
        { 
            bubbleText.text = message;
        }

        manager = GameManager.instance;

    }

    private void Update()
    {
        if (bubbleShowing && bubbleObject != null)
        {
            manager.guiAlwaysFacePlayer(bubbleObject);
        }
    }

    public void Interacted() 
    {

        bubbleShowing = !bubbleShowing;

        if (bubbleObject != null)
            bubbleObject.SetActive(bubbleShowing);

        if (bubbleText != null)
            bubbleText.text = message;

        if (useGameManagerDialog && GameManager.instance != null)
            manager.addDialog(message);




    }

    public void StopInteraction()
    {
        if (bubbleObject != null)
        {

            bubbleShowing = false;
            bubbleObject.SetActive(false);
        }
    }

}
