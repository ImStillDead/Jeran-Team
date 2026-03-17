using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UI;

public class NPCInteract : MonoBehaviour, iInteract
{
    [SerializeField] private GameObject bubbleObject;
    [SerializeField] private TMP_Text bubbleText;

    [SerializeField] private GameObject interactPrompt;

    [TextArea(2, 5)]
    [SerializeField] private string[] messages;

    [System.Serializable]
    public class PlayerChoice
    {
        public string choiceText;

        [TextArea(2, 5)]
        public string responseText;
    }

    private bool bubbleShowing = false;
    private int currentPage = 0;

    private GameManager manager;

    void Start()
    {
        manager = GameManager.instance;

        if (bubbleObject != null)
            bubbleObject.SetActive(false);

        if (bubbleText != null && messages != null && messages.Length > 0)
            bubbleText.text = messages[0];
    }

    void Update()
    {
        if (bubbleShowing && bubbleObject != null && manager != null)
        {
            manager.guiAlwaysFacePlayer(bubbleObject);
        }
    }

    public void Interacted()
    {
        if (messages == null || messages.Length == 0)
            return;

        // First interaction opens the bubble and shows first page
        if (!bubbleShowing)
        {
            bubbleShowing = true;
            currentPage = 0;

            if (bubbleObject != null)
                bubbleObject.SetActive(true);

            ShowCurrentPage();
            return;
        }

        // Go to next page
        currentPage++;

        // If there are still pages left, show next page
        if (currentPage < messages.Length)
        {
            ShowCurrentPage();
        }
        else
        {
            // End of dialogue
            StopInteraction();
        }
    }

    private void ShowCurrentPage()
    {
        if (bubbleText != null)
            bubbleText.text = messages[currentPage];
    }

    public void StopInteraction()
    {
        bubbleShowing = false;
        currentPage = 0;

        if (bubbleObject != null)
            bubbleObject.SetActive(false);
    }
}