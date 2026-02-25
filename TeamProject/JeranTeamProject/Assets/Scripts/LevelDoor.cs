using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    [SerializeField] GameObject button;
    bool playerInTrigger;
    void Start()
    {

    }
    void Update()
    {
        if (Input.GetButtonDown("Interact") && playerInTrigger && GameManager.instance.objectiveCompleted)
        {
            GameManager.instance.youWin();
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

