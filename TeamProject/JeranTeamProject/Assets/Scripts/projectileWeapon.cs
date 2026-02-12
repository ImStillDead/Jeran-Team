using System.Collections;
using UnityEngine;


public class projectileWeapon : MonoBehaviour
{
    [SerializeField] GameObject projectile_type;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int currentAmmo;
    [SerializeField] int projectileSpeed;
    [SerializeField] int reloadSpeed;
    [SerializeField] int maxAmmoCount;
    [SerializeField] float fireRate;
    [SerializeField] Transform shootPOS;

    float shootTimer;
    bool isReloading;

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer >= fireRate && currentAmmo > 0 && !isReloading)
        {
            shoot();
            currentAmmo--;

            ammocount();

        }

        if (currentAmmo <= 0 && !isReloading) {
            StartCoroutine(Reloadtime());
        }


    }
    void shoot()
    {

        shootTimer = 0;
        Instantiate(projectile_type, shootPOS.position, transform.rotation);
    }

    void ammocount()
    {
        GameManager.instance.ammocount(currentAmmo, maxAmmoCount);
    }

    IEnumerator Reloadtime()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadSpeed); 
        currentAmmo = maxAmmoCount;
        ammocount();

        isReloading = false;

    }       


}
