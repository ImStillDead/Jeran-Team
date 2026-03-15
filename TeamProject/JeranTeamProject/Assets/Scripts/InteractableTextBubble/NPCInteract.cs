using UnityEngine;
using TMPro;

public class NPCInteract : MonoBehaviour, iInteract
{
    [SerializeField] private GameObject bubbleObject;
    [SerializeField] private TMP_Text bubbleText;

    [TextArea(2, 5)]
    [SerializeField] private string[] message;

    [SerializeField] private bool useGameManagerDialog = true;

    private bool bubbleShowing = false;
    private int currentPage = 0;

    GameManager manager;

    void Start()
    {
        manager = GameManager.instance;

        if (bubbleObject != null)
        {
            bubbleObject.SetActive(false);
        }

        if (bubbleText != null && message != null && message.Length > 0)
        { 
            bubbleText.text = message[0];
        }
    }

    private void Update()
    {
        if (bubbleShowing && bubbleObject != null && manager != null)
        {
            manager.guiAlwaysFacePlayer(bubbleObject);
        }
    }

    public void Interacted() 
    {
        // If there is no message, do nothing
        if (message == null || message.Length == 0)
            return;
        // If the bubble is not currently showing, show it and display the first page of the message
        if (!bubbleShowing)
        {
            bubbleShowing = true;
            if (bubbleObject != null)
            {
                bubbleObject.SetActive(true);
            }
            ShowPage();
            return;
        }
        // If the bubble is currently showing, advance to the next page of the message
        currentPage++;
        if(currentPage < message.Length)
        {
            ShowPage();
        }
        else
        {
            StopInteraction();
        }
    }
    private void ShowPage()
    {
        if (bubbleText != null)
            bubbleText.text = message[currentPage];
        if (useGameManagerDialog && manager != null)
        {
            
            manager.addDialog(message[currentPage]);
        }

    }

    public void StopInteraction()
    {
        bubbleShowing = false;
        currentPage = 0;

        if (bubbleObject != null)
            bubbleObject.SetActive(false);
    }
}
