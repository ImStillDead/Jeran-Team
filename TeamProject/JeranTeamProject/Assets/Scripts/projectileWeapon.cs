using UnityEngine;


public class projectileWeapon : MonoBehaviour
{
    [SerializeField] GameObject projectile_type;
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




    }




}
