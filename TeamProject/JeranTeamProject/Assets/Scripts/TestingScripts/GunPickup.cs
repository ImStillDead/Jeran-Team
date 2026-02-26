using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats gun;

    private void OnTriggerEnter(Collider other)
    {
        IGunPickup pick = other.GetComponent<IGunPickup>();

        if(pick != null )
        {
            gun.currentAmmo = gun.magSizeMax;
            pick.GetGunStats(gun);
            Destroy(gameObject);
        }
    }
}
