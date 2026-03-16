using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UI;

public class NPCInteract : MonoBehaviour, iInteract
{
    [System.Serializable]
    public class PlayerChoice
    {
        public string choiceText;
        [TextArea(2, 5)]
        public string responseText;
    }
    [SerializeField] private GameObject bubbleObject;
    [SerializeField] private TMP_Text bubbleText;

    [TextArea(2, 5)]
    [SerializeField] private string[] messages;

    [SerializeField] private bool useChoices = false;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private TMP_Text[] choiceButtonTexts;
    [SerializeField] private PlayerChoice[] playerChoices;

    private bool bubbleShowing = false;
    private bool showingChoices = false;
    private bool showingResponse = false;
    private int currentPage = 0;

    GameManager manager;

    void Start()
    {
        manager = GameManager.instance;

        if (bubbleObject != null)
            bubbleObject.SetActive(false);

        if (choicePanel != null)
            choicePanel.SetActive(false);

        if (bubbleText != null && messages != null && messages.Length > 0)
            bubbleText.text = messages[0];

        SetupChoiceButtons();

    }

    private void Update()
    {
        // If the bubble is showing, make sure it always faces the player
        if (bubbleShowing && bubbleObject != null && manager != null)
        {
            manager.guiAlwaysFacePlayer(bubbleObject);
        }
    }

    public void Interacted()
    {
        // If there is no message, do nothing
        if (messages == null || messages.Length == 0)
            return;
        // If the bubble is not currently showing, show it and display the first page of the message
        if (!bubbleShowing)
        {
            bubbleShowing = true;
            currentPage = 0;
            showingChoices = false;
            showingResponse = false;

            if (bubbleObject != null)
                bubbleObject.SetActive(true);

            ShowCurrentPage();
            return;
        }
        if (showingChoices)
            return;
        if (showingResponse)
        {
            StopInteraction();
            return;
        }
        currentPage++;

        if (currentPage < messages.Length)
        {
            ShowCurrentPage();
        }
        else
        {
            // End of NPC lines
            if (useChoices && playerChoices != null && playerChoices.Length > 0)
            {
                ShowChoices();
            }
            else
            {
                StopInteraction();
            }
        }
    }
    
    private void ShowCurrentPage()
    {
        if (bubbleText != null)
            bubbleText.text = messages[currentPage];
    }

    private void SetupChoiceButtons()
    {
        if (choiceButtons == null || choiceButtonTexts == null)
            return;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            int index = i;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => SelectChoice(index));
        }
    }

    private void ShowChoices()
    {
        showingChoices = true;

        if (choicePanel != null)
            choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (playerChoices != null && i < playerChoices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);

                if (i < choiceButtonTexts.Length && choiceButtonTexts[i] != null)
                    choiceButtonTexts[i].text = playerChoices[i].choiceText;
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }
    private void SelectChoice(int index)
    {
        if (playerChoices == null || index >= playerChoices.Length)
            return;

        showingChoices = false;
        showingResponse = true;

        if (choicePanel != null)
            choicePanel.SetActive(false);

        if (bubbleText != null)
            bubbleText.text = playerChoices[index].responseText;
    }

    public void StopInteraction()
    {
        bubbleShowing = false;
        showingChoices = false;
        showingResponse = false;
        currentPage = 0;

        if (bubbleObject != null)
            bubbleObject.SetActive(false);

        if (choicePanel != null)
            choicePanel.SetActive(false);

    }
}
