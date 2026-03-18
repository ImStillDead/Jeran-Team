using UnityEngine;

public class Objective : MonoBehaviour
{
    GameManager gamemanger = GameManager.instance;

    [SerializeField] GameObject button;
    public Light buttonLight;
    bool playerInTrigger;
    bool start;
    void Start()
    {
        start = false;
    }
    void Update()
    {
        
        if (Input.GetButtonDown("Interact") && playerInTrigger && !start)
        {
            GameManager.instance.StartObjective();
            GameManager.instance.objectiveLight = buttonLight;
            start = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            button.SetActive(true);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            button.SetActive(false);
        }
    }
}
