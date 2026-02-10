using UnityEngine;


public class projectileWeapon : MonoBehaviour
{
    [SerializeField] GameObject projectile_type;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int currentAmmo;
    [SerializeField] int projectileSpeed;
    [SerializeField] int reloadSpeed;
    [SerializeField] int maxAmmoCount;
    [SerializeField] int fireRate;
    [SerializeField] Transform shootPOS;

    float shootTimer;

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer >= fireRate)
        {
            shoot();
        }


    }
    void shoot()
    {
        shootTimer = 0;
        Instantiate(projectile_type, shootPOS.position, transform.rotation);
    }



}
