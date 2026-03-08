using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class chest : MonoBehaviour , iInteract
{
    [SerializeField] GameObject box;
    [SerializeField] Transform targetTrans;
    [SerializeField] Transform hingePoint;
    [SerializeField] Transform itemLocation;
    [SerializeField] int amountOfMaxItems;
    [SerializeField] List<GameObject> items;

    private GameObject itemGiven;
    private int itemsSpawned = 0;
    private float openAngle = 45f;
    private bool isOpen = true;

    public void Interacted()
    {
        isOpen = !isOpen;





        if (isOpen == false)
        {

            openChest();

            if (items.Count == 0)
            {
                Debug.LogWarning("there is nothing in the list of items, please add items");
                return;
            }
             
            else
            {
                
                if (itemsSpawned < amountOfMaxItems)
                {
                    if(itemGiven == null)
                    {
                         itemGiven = Instantiate(items[randomNumberPicker()], itemLocation.position, Quaternion.identity);
                         GameManager.instance.addDialog("i found a " + itemGiven.name);
                    }

                    itemsSpawned++;
                    Debug.Log("Item spawned: " + itemsSpawned);

                    if (itemsSpawned >= amountOfMaxItems)
                    {
                        Debug.Log("Maximum items spawned, no more items will appear.");
                    }
                }
            }

            if (itemGiven != null)
            {
                itemGiven.gameObject.SetActive(true);

            }
        }

        else if (isOpen == true)
        {
            closeChest();

            if(itemGiven != null) 
            itemGiven.gameObject.SetActive(false);
        }
    }
    void openChest()
    {   
        targetTrans.RotateAround(hingePoint.position, hingePoint.right, openAngle);
        Debug.Log("Object opened");
    }
    void closeChest()
    {
        targetTrans.RotateAround(hingePoint.position, hingePoint.right, -openAngle);
        Debug.Log("Object closed");
    }

    int randomNumberPicker()
    {
        int item = Random.Range(0, items.Count);

        return item;
    }

}
