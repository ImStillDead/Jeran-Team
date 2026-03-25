using UnityEngine;

public class supportingGunScript : MonoBehaviour
{

    GunStats defaultGun;

    private void Start()
    {
        defaultGun.gunRarity = GunRarity.Common;
    }


    public void pickUpScope()
    {






    }

    


    public void gunTierStats(GunStats gun)
    {
        // Base values from the default gun
        gun.hipSpread = defaultGun.hipSpread;
        gun.adsSpread = defaultGun.adsSpread;
        gun.shootRate = defaultGun.shootRate;
        gun.magSizeMax = defaultGun.magSizeMax;

        // Damage multiplier per rarity
        float largeMultiplier = 1f; // default for Common
        float smallMultiplier = 1f;

        switch (gun.gunRarity)
        {
            case GunRarity.Common:
                largeMultiplier = 1f;
                smallMultiplier = 1f;
                break;
            case GunRarity.Uncommon:
                largeMultiplier = 1.5f;
                smallMultiplier = 1.05f;
                break;
            case GunRarity.Rare:
                largeMultiplier = 2f;
                smallMultiplier = 1.15f;
                break;
            case GunRarity.Perfected:
                largeMultiplier = 2.5f;
                smallMultiplier = 1.45f;
                break;
            case GunRarity.Exotic:
                largeMultiplier = 3f;
                smallMultiplier = 1.75f;
                break;

        }

        // Apply multiplier to damage
        gun.bullet.damageAmount = (int)(defaultGun.bullet.damageAmount * largeMultiplier);
        gun.hipSpread /= largeMultiplier;
        gun.adsSpread /= largeMultiplier;
        gun.shootRate *= smallMultiplier;
        gun.magSizeMax = Mathf.CeilToInt(defaultGun.magSizeMax * smallMultiplier);
    }




}
