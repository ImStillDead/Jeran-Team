using Unity.VisualScripting;
using UnityEngine;

public class chest : MonoBehaviour , iInteract
{
    [SerializeField] GameObject box;
    [SerializeField] Transform targetTrans;
    [SerializeField] Transform hingePoint;
    [SerializeField] Transform itemLocation;
    [SerializeField] GameObject item;
    [SerializeField] int amountOfMaxItems;

    private int itemsSpawned = 0;
    private float openAngle = 45f;
    private bool isOpen = true;

    public void Interacted()
    {


        isOpen = !isOpen;



        if (isOpen == false)
        {

            openChest();
            if (itemsSpawned < amountOfMaxItems)
            {
                GameObject spawnedItem = Instantiate(item, itemLocation.position, Quaternion.identity);

                itemsSpawned++;
                Debug.Log("Item spawned: " + itemsSpawned);

                if (itemsSpawned >= amountOfMaxItems)
                {
                    Debug.Log("Maximum items spawned, no more items will appear.");
                }
            }
        }

        else if (isOpen == true)
        {
            closeChest();

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


}
