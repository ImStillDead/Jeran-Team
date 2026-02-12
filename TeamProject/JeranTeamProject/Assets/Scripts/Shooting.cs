using UnityEngine;
using System.Collections;


public class Shooting : MonoBehaviour
{

    public static Shooting instance;
    
    // [SerializeFields] for variables that we want to edit in Unity
    [SerializeField] int shootDamage;
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
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
       
        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            Shoot();
        }
       
        if (currentAmmo <= 0 && !reloading)
        {       
            StartCoroutine(Reload());       
        }

    }

    public void Shoot()
    {
        if(!reloading)
        {
            shootTimer = 0;
            Instantiate(bullet, shootPos.position, transform.rotation);
            currentAmmo = currentAmmo - 1;
        }

    }

    IEnumerator Reload()
    {

        reloading = true;

        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;

        reloading = false;
    }
}
