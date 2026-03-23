using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats gun;
    GameObject gunModel;
    private IGunPickup pick = null;
    private bool canSwap;
    private void Start()
    {
        if(gunModel == null)
        {
            gunModel = Instantiate(gun.gunModel, this.transform);
            gunModel.transform.localScale *= 3;
            gunModel.transform.localPosition = Vector3.zero;
        }
        GameManager.instance.pickUpObjects.Add(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        pick = other.GetComponent<IGunPickup>();

        if(pick != null)
        {
            gun.currentAmmo = gun.magSizeMax;
            pick.GetGunStats(gun);
            if (Shooting.instance.gunList.Count < 2)
            {
                Destroy(gameObject);
                GameManager.instance.pickUpObjects.Remove(this.gameObject);
            }
            else
            {
                canSwap = true;
                Debug.Log("Cannot Pick Up, Would You like to Swap?");
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        pick = other.GetComponent<IGunPickup>();
        if (pick != null && canSwap)
        {
            if (Input.GetButtonDown("Interact"))
            {
                SwapGun();
            }
        }
        else
        {
            canSwap = true;
        }
    }
    public void SwapGun()
    {
        GunStats tempGun = Shooting.instance.Swap();
        GameManager.instance.playerScript.SwapGunPickup(gun);
        gun = tempGun;
        Destroy(gunModel);
        gunModel = Instantiate(gun.gunModel, this.transform);
        gunModel.transform.localPosition = Vector3.zero;
        gunModel.transform.localScale = gun.scale * 3;
        canSwap = false;
    }
}
