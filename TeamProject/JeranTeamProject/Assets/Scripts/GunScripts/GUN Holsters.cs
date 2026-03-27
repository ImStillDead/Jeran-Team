using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class GUNHolsters : MonoBehaviour
{
    [SerializeField] Transform gunPos1;
    [SerializeField] Transform gunPos2;

    public List<GunStats> gunList = new List<GunStats>();

    private GameObject activeGunModel;
    private GameObject holsterGunModel;

    public PlayerController player;

    private GunStats activeGunStats;
    private GunStats holsterGunStats;

    bool canSwap = true;
    float swapCooldown = 0.2f;
    cardHolder holder;
    public bool swapCharacters;

    void Start()
    {
        if (player == null) return;

        // Only proceed if there are at least 2 guns
        if (gunList.Count < 2)
        {
            //Debug.LogWarning("Gun list can only have 2 guns!");
            return;
        }
    }



    public void SwapHolsterGuns()
    {
        if (!canSwap) return;

        canSwap = false;
        Invoke(nameof(ResetSwap), swapCooldown);

        if (holsterGunStats == null || holsterGunModel == null || activeGunModel == null)
        {
            Debug.LogWarning("Cannot swap: one of the gun models or stats is null!");
            return;
        }

        GunStats tempStats = activeGunStats;
        activeGunStats = holsterGunStats;
        holsterGunStats = tempStats;

        gunList[0] = activeGunStats;
        gunList[1] = holsterGunStats;

        Transform tempParent = activeGunModel.transform.parent;
        activeGunModel.transform.parent = holsterGunModel.transform.parent;
        holsterGunModel.transform.parent = tempParent;

        activeGunModel.transform.localPosition = Vector3.zero;
        activeGunModel.transform.localRotation = Quaternion.Euler(0, 180, 0);

        holsterGunModel.transform.localPosition = Vector3.zero;
        holsterGunModel.transform.localRotation = Quaternion.Euler(0, 180, 0);

        var tempModel = activeGunModel;
        activeGunModel = holsterGunModel;
        holsterGunModel = tempModel;

        player.updateSwapGun(gunList[0]);

    }

    public void AddGun(GunStats newGun)
    {
        if (gunList.Count >= 2) return;
        gunList.Add(newGun);

        if (gunList.Count == 1)
        {
            activeGunStats = newGun;
            activeGunModel = Instantiate(activeGunStats.gunModel, gunPos1);
            activeGunModel.transform.localPosition = Vector3.zero;
            activeGunModel.transform.localRotation = Quaternion.Euler(0,180,0);
            Shooting.instance.locationFinder();
        }
        else if (gunList.Count == 2)
        {
            holsterGunStats = newGun;
            holsterGunModel = Instantiate(holsterGunStats.gunModel, gunPos2);
            holsterGunModel.transform.localPosition = Vector3.zero;
            holsterGunModel.transform.localRotation = Quaternion.Euler(0, 180, 0);
            Shooting.instance.locationFinder();
        }

    }


    public void SwapWithPickup(GunStats pickupGun, GunPickup pickup)
    {
        if (activeGunModel != null)
            Destroy(activeGunModel);

        GunStats oldGun = activeGunStats;


        activeGunStats = pickupGun;
        gunList[0] = activeGunStats;

        player.UpdateActiveGun(activeGunStats);

        activeGunModel = Instantiate(activeGunStats.gunModel, gunPos1);
        activeGunModel.transform.localPosition = Vector3.zero;

        pickup.UpdatePickup(oldGun);


    }

    public void ClearGunModels()
    {
        if (activeGunModel != null)
        {
            Destroy(activeGunModel);
            activeGunModel = null;
        }

        if (holsterGunModel != null)
        {
            Destroy(holsterGunModel);
            holsterGunModel = null;
        }

        activeGunStats = null;
        holsterGunStats = null;

        gunList.Clear();
    }

    public GunStats GetActiveGun()
    {
        return activeGunStats;
    }
    void ResetSwap()
    {
        canSwap = true;
    }
}
