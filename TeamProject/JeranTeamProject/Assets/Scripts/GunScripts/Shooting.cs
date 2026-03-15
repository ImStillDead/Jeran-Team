using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class Shooting : MonoBehaviour
{

    public static Shooting instance;

    // [SerializeFields] for variables that we want to edit in Unity
    [SerializeField] GameObject gunModel;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] float shootRate;
    [SerializeField] int magSizeMax;
    [SerializeField] float reloadTime;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    [SerializeField] AudioClip[] aud;
    [SerializeField] Bullet bulletScript;

    //Public variables
    public List<GunStats> gunList = new List<GunStats>();
    public int currentAmmo;
    public int startingMaxAmmo;
    public static float shootTimer;
    public float volume;

    public float spread;

    public Recoil recoil;

    // Other Variables
    bool reloading;
    int activeGun;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        currentAmmo = magSizeMax;  // Sets currentAmmo equal to the maxAmmo

        recoil = GameObject.Find("CameraRot/CameraRecoil").GetComponent<Recoil>();

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
        if (currentAmmo <= 0 && !reloading)
        {       
            StartCoroutine(Reload());       
        }

        if(Input.GetButton("Reload") && !reloading)
        {
            StartCoroutine(Reload());
        }

    }
    public void callAmmo()
    {
        GameManager.instance.ammocount(currentAmmo, magSizeMax);
        if (gunList.Count > 0)
        {
            gunList[activeGun].currentAmmo = currentAmmo;
        }
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
    public void changeGun(int gunPos)
    {
        activeGun = gunPos;
        currentAmmo = gunList[gunPos].currentAmmo;
        magSizeMax = gunList[gunPos].magSizeMax;
        bulletScript = gunList[gunPos].bullet;
        shootRate = gunList[gunPos].shootRate;
        reloadTime = gunList[gunPos].reloadTime;
        aud = gunList[gunPos].aud;
        volume = gunList[gunPos].shotSoundVol;

        spread = gunList[gunPos].spread;

        gunModel.GetComponent<MeshFilter>().sharedMesh = instance.gunList[gunPos].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = instance.gunList[gunPos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
        gunModel.transform.localScale = gunList[gunPos].scale;
        gunModel.transform.localPosition = gunList[gunPos].postion;
        gunModel.transform.localRotation = gunList[gunPos].rotation;
        shootPos.transform.localPosition = gunList[gunPos].shootPos.transform.localPosition;
        shootPos.transform.localRotation = gunList[gunPos].shootRotate;
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
            GameManager.instance.playerScript.playAudio(aud[0], volume);


            Quaternion spreadRotation = shootPos.transform.rotation * 
                Quaternion.Euler(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);


            Instantiate(bullet, shootPos.position, spreadRotation);
            
            
            currentAmmo = currentAmmo - 1;
            callAmmo();
            
            recoil.RecoilFire();
        }

    }

    // Called in Update if the currentAmmo is less than or equal to 0 and the player is not reloading
    IEnumerator Reload()
    {
        reloading = true;                               // Sets reloading to true to stop the player from firing
        yield return new WaitForSeconds(reloadTime);    // Waits for a set amount of time determined by the reloadTime
        currentAmmo = magSizeMax;                   // Sets currentAmmo equal to the max ammo
        callAmmo();
        reloading = false;                              // Sets reloading back to false so the player can shoot again
    }
}
