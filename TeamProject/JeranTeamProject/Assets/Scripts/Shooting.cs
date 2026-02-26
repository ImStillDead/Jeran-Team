using UnityEngine;
using System.Collections;
using Unity.VisualScripting;


public class Shooting : MonoBehaviour
{

    public static Shooting instance;

    // [SerializeFields] for variables that we want to edit in Unity
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject magModel;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] float shootRate;
    [SerializeField] int magSizeMax;
    [SerializeField] float reloadTime;
    [SerializeField] public GameObject bullet;
    [SerializeField] Transform shootPos;

    [SerializeField] AudioClip[] aud;
    [SerializeField] Bullet bulletScript;

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
        currentAmmo = magSizeMax;  // Sets currentAmmo equal to the maxAmmo
        callAmmo();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
       
        /*  Gets the input of the fire button and checks if the shoot timer is greater than
            equal to the shoot rate. If it is it calls the Shoot() method(function) */
        if (Input.GetButton("Fire1") && shootTimer >= shootRate && currentAmmo > 0)
        {
            Shoot();
        }
       
        /*  Checks to see if currentAmmo is less than or equal to 0 and if the player is not reloading.
            If so, it calls the Reload() method(function) */    
        if (currentAmmo <= 0 && !reloading && maxAmmo > 0)
        {       
            StartCoroutine(Reload());       
        }

        if(Input.GetButton("Reload") && !reloading && maxAmmo > 0)
        {
            StartCoroutine(Reload());
        }

    }
    public void callAmmo()
    {
        GameManager.instance.ammocount(currentAmmo, magSizeMax, maxAmmo);
    }
    // Called in Update if the Fire1 button (Left Click) is pressed
    public void changeBullet()
    {
        bullet.GetComponent<Damage>().damageAmount = bulletScript.damageAmount;
        bullet.GetComponent<Damage>().damageRate = bulletScript.damageRate;
        bullet.GetComponent<Damage>().destroyTime = bulletScript.destroyTime;
        bullet.GetComponent<Damage>().hitEffect = bulletScript.hitEffect;
        bullet.GetComponent<Damage>().speed = bulletScript.speed;
    }
    public void changeGun(GunStats gunStats)
    {
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunStats.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStats.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        magModel.GetComponent<MeshFilter>().sharedMesh = gunStats.magModel.GetComponent<MeshFilter>().sharedMesh;
        magModel.GetComponent<MeshRenderer>().sharedMaterial = gunStats.magModel.GetComponent<MeshRenderer>().sharedMaterial;

        currentAmmo = gunStats.currentAmmo;
        magSizeMax = gunStats.magSizeMax;
        maxAmmo = gunStats.maxAmmo;
        bulletScript = gunStats.bullet;
        shootRate = gunStats.shootRate;
        reloadTime = gunStats.reloadTime;
        changeBullet();
        callAmmo();
    }

    public void Shoot()
    {
        /*  Checks to see if the player is not reloading. If they are not, it fires a projectile
            and decreases current ammo by 1 */
        if(!reloading)
        {
            shootTimer = 0;
            Instantiate(bullet, shootPos.position, transform.rotation);
            currentAmmo = currentAmmo - 1;
            callAmmo();
        }

    }

    // Called in Update if the currentAmmo is less than or equal to 0 and the player is not reloading
    IEnumerator Reload()
    {
        reloading = true;                               // Sets reloading to true to stop the player from firing

        yield return new WaitForSeconds(reloadTime);    // Waits for a set amount of time determined by the reloadTime
        if(maxAmmo >= magSizeMax)                       // check for carried ammo
        {
            maxAmmo -= magSizeMax - currentAmmo;
            currentAmmo = magSizeMax;                   // Sets currentAmmo equal to the max ammo
        }
        else
        {
            if (currentAmmo > 0)
            {
                currentAmmo += maxAmmo;                 //add ammo to current
                maxAmmo = currentAmmo - magSizeMax;     //subtract extra from current ammo if any;
                if(currentAmmo > magSizeMax)
                {
                    currentAmmo = magSizeMax;               //reset current to max mag size
                }
                if(maxAmmo < 0)
                {
                    maxAmmo = 0;
                }
            }
            else
            {
                currentAmmo = maxAmmo;
                maxAmmo = 0;
            }
            
        }
        callAmmo();
        reloading = false;                              // Sets reloading back to false so the player can shoot again
    }
}
