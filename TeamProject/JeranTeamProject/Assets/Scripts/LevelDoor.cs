using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] GameObject Lights;
    bool playerInTrigger;
    void Start()
    {
        GameManager.instance.doorLights = Lights;
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

