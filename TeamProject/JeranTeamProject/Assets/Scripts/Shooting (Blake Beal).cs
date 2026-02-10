using UnityEngine;
using System.Collections;


public class Shooting : MonoBehaviour
{

    public static Shooting instance;
    // [SerializeFields] for variables that we want to edit in Unity
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] public static float shootRate;
    [SerializeField] int magSizeMax;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    //Public variables
    public int currentAmmo;
    public int maxAmmo;

    // Other Variables
    public static float shootTimer;

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

    }

    public void Shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, shootPos.position, transform.rotation);
        currentAmmo--;


    }
}
