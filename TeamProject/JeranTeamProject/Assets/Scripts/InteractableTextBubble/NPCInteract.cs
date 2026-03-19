using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCInteract : MonoBehaviour, iInteract
{
    [SerializeField] private GameObject bubbleObject;
    [SerializeField] private TMP_Text bubbleText;

    [SerializeField] private GameObject chatWindow;
    [SerializeField] private TMP_Text chatWindowText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;

    [SerializeField] private GameObject skipConfirmPanel;
    [SerializeField] private Button skipYesButton;
    [SerializeField] private Button  skipNoButton;

    [SerializeField] private GameObject interactPrompt;

    [TextArea(2, 5)]
    [SerializeField] private string[] messages;


    private bool bubbleShowing = false;
    private int currentPage = 0;

    private GameManager manager;

    void Start()
    {
        manager = GameManager.instance;

        if (bubbleObject != null)
            bubbleObject.SetActive(false);
        if (chatWindow != null)
            chatWindow.SetActive(false);
        if (skipConfirmPanel != null)
            skipConfirmPanel.SetActive(false);
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (bubbleText != null && messages != null && messages.Length > 0)
            bubbleText.text = messages[0];
        if (chatWindowText != null && messages != null && messages.Length > 0)
            chatWindowText.text = messages[0];

        if (nextButton != null)
            nextButton.onClick.AddListener(NextPage);
        if (skipButton != null)
            skipButton.onClick.AddListener(OpenSkipConfirm);
        if (skipYesButton != null)
            skipYesButton.onClick.AddListener(ConfirmSkip);
        if (skipNoButton != null)
            skipNoButton.onClick.AddListener(CancelSkip);
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
            if (chatWindow != null)
                chatWindow.SetActive(true);
            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ShowCurrentPage(); 
        }
    }

    private void ShowCurrentPage()
    {
        if (bubbleText != null)
            bubbleText.text = messages[currentPage];

        if (chatWindowText != null)
            chatWindowText.text = messages[currentPage];
    }
    public void NextPage()
    {
        currentPage++;
        if (currentPage < messages.Length)
        {
            ShowCurrentPage();
        }
        else
        {
            StopInteraction();
        }
    }
    public void OpenSkipConfirm()
    {
        if (skipConfirmPanel != null)
            skipConfirmPanel.SetActive(true);
    }
    public void CancelSkip()
    {
        if (skipConfirmPanel != null)
            skipConfirmPanel.SetActive(false);
    }
    public void ConfirmSkip()
    {
        StopInteraction();
    }
    public void StopInteraction()
    {
        bubbleShowing = false;
        currentPage = 0;

        if (bubbleObject != null)
            bubbleObject.SetActive(false);
        if (chatWindow != null)
            chatWindow.SetActive(false);
        if (skipConfirmPanel != null)
            skipConfirmPanel.SetActive(false);
        if (interactPrompt != null)
            interactPrompt.SetActive(true);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
            StopInteraction();
        }
    }
}
