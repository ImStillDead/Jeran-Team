using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public class Shooting : MonoBehaviour
{

    public static Shooting instance;
    
    // [SerializeFields] for variables that we want to edit in Unity
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] float shootRate;
    [SerializeField] int magSizeMax;
    [SerializeField] float reloadTime;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    //Public variables
    public int currentAmmo;
    public int maxAmmo;
    public static float shootTimer;

    // Other Variables
    bool reloading;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        maxAmmo = magSizeMax;   // Sets maxAmmo to the maximum mag size
        currentAmmo = maxAmmo;  // Sets currentAmmo equal to the maxAmmo
        GameManager.instance.ammocount(currentAmmo, magSizeMax);
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
       
        /*  Gets the input of the fire button and checks if the shoot timer is greater than
            equal to the shoot rate. If it is it calls the Shoot() method(function) */
        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            Shoot();
        }
       
        /*  Checks to see if currentAmmo is less than or equal to 0 and if the player is not reloading.
            If so, it calls the Reload() method(function) */    
        if (currentAmmo <= 0 && !reloading)
        {       
            StartCoroutine(Reload());       
        }

    }

    // Called in Update if the Fire1 button (Left Click) is pressed
    public void Shoot()
    {
        /*  Checks to see if the player is not reloading. If they are not, it fires a projectile
            and decreases current ammo by 1 */
        if(!reloading)
        {
            shootTimer = 0;
            Instantiate(bullet, shootPos.position, transform.rotation);
            currentAmmo = currentAmmo - 1;
            GameManager.instance.ammocount(currentAmmo, magSizeMax);
        }

    }

    // Called in Update if the currentAmmo is less than or equal to 0 and the player is not reloading
    IEnumerator Reload()
    {

        reloading = true;                               // Sets reloading to true to stop the player from firing

        yield return new WaitForSeconds(reloadTime);    // Waits for a set amount of time determined by the reloadTime
        currentAmmo = maxAmmo;                          // Sets currentAmmo equal to the max ammo

        reloading = false;                              // Sets reloading back to false so the player can shoot again
    }
}
