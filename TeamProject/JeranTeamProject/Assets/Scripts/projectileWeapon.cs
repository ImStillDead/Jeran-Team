using UnityEngine;


public class projectileWeapon : MonoBehaviour
{
    [SerializeField] GameObject projectile_type;
    [SerializeField] Rigidbody rb;
    [SerializeField] int amount_of_projectiles;
    [SerializeField] int projectile_speed_mult;
    [SerializeField] int reload_speed;
    [SerializeField] int magazine_amount;
    [SerializeField] int fireRate;
    [SerializeField] Transform shootPOS;

    float shootTimer;



    void Update()
    {
        shootTimer += Time.deltaTime;




    }




}
