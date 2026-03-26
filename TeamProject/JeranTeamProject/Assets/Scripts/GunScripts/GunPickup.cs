using UnityEngine;

public class GunPickup : MonoBehaviour, IPickup, iInteract
{
    [SerializeField] private GunStats gun;
    private GameObject gunModel;
    private GUNHolsters holster;
    bool pickedupBefore = false;

    private void Start()
    {
        holster = Shooting.instance?.GetComponentInParent<GUNHolsters>();

        if (gunModel == null && gun != null && gun.gunModel != null)
        {
            gunModel = Instantiate(gun.gunModel, transform);
            gunModel.transform.localPosition = Vector3.zero;
            gunModel.transform.localScale = gun.scale * 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            if (Shooting.instance != null && gun != null)
            {
                             
                if(holster.gunList.Count < 2)
                {
                    Shooting.instance.SetGun(gun);
                    pickedupBefore = true;
                    Destroy(gameObject);
                }
                else
                {
                    holster.SwapWithPickup(gun, this);
                    //holster.SwapHolsterGuns();

                }

            }


            else if (holster.gunList.Count >= 2)
            {
                Debug.Log("gunlist is full press interact to swap weapons");
            }

        }
    }

    public void UpdatePickup(GunStats newGun)
    {
        gun = newGun;

        if (gunModel != null)
            Destroy(gunModel);

        if (gun != null && gun.gunModel != null)
        {
            gunModel = Instantiate(gun.gunModel, transform);
            gunModel.transform.localPosition = Vector3.zero;
            gunModel.transform.localScale = gun.scale * 3;
        }
    }

    public void Interacted()
    {
        holster.SwapWithPickup(gun, this);
    }
}