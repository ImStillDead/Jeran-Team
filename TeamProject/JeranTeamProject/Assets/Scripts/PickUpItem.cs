using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] Pickups item;
    public void OnTriggerEnter(Collider other)
    {
        IPickup pick = other.GetComponent<IPickup>();

        if (pick != null)
        {
            pick.pickUpObject(item);
            Destroy(gameObject);
        }
    }
}

