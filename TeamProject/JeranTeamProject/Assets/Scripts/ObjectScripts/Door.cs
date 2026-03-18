using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject model;
    [SerializeField] GameObject button;
    bool playerInTrigger;
    bool enemyInTrigger;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetButtonDown("Interact") && playerInTrigger)
        {
            if (model.activeSelf)
            {
                model.SetActive(false);
            }
            else
            {
                model.SetActive(true);
            }
        }
        if (enemyInTrigger)
        {
            model.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            button.SetActive(true);
        }
        if (other.CompareTag("Enemy"))
        {
            enemyInTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            model.SetActive(true);
            playerInTrigger = false;
            button.SetActive(false);
        }
        if (other.CompareTag("Enemy"))
        {
            enemyInTrigger = false;
            if (!playerInTrigger)
            {
                model.SetActive(true);
            }
        }
    }
}
