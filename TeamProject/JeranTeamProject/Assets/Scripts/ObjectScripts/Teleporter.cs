using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] GameObject model;
    [SerializeField] GameObject button;
    [SerializeField] GameObject landing;
    private static List<Teleporter> teleporters = new List<Teleporter>();
    int TPindex;
    bool playerInTrigger;
    void Start()
    {
        teleporters.Add(this);
        TPindex = 0;
    }
    void Update()
    {
        if (Input.GetButtonDown("Interact") && playerInTrigger)
        {
            DataManager.manager.player.transform.position = teleporters[TPindex].transform.position;
            TPindex++;
            if(TPindex == teleporters.Count - 1)
            {
                TPindex = 0;
            }
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
            model.SetActive(true);
            playerInTrigger = false;
            button.SetActive(false);
        }
        
    }
}
