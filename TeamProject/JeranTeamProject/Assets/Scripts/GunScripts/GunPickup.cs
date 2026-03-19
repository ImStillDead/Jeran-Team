using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunStats gun;
    private IGunPickup pick = null;
    private bool canSwap;
    private void Start()
    {
        this.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        this.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
        this.transform.localScale = gun.scale * 3;
        this.transform.localRotation = gun.rotation;
        GameManager.instance.pickUpObjects.Add(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        pick = other.GetComponent<IGunPickup>();

        if(pick != null)
        {
            gun.currentAmmo = gun.magSizeMax;
            pick.GetGunStats(gun);
            if (GameManager.instance.playerScript.canPickup)
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
        this.GetComponent<MeshFilter>().sharedMesh = Shooting.instance.Swap().gunModel.GetComponent<MeshFilter>().sharedMesh;
        this.GetComponent<MeshRenderer>().sharedMaterial = Shooting.instance.Swap().gunModel.GetComponent<MeshRenderer>().sharedMaterial;
        this.transform.localScale = Shooting.instance.Swap().scale * 3;
        this.transform.localRotation = Shooting.instance.Swap().rotation;
        GameManager.instance.playerScript.SwapGunPickup(gun);
        gun = tempGun;
        canSwap = false;
    }
}
