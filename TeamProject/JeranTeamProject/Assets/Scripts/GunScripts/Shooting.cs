using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class Shooting : MonoBehaviour
{

    public static Shooting instance;

    // [SerializeFields] for variables that we want to edit in Unity
    [SerializeField] GameObject gunModel;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] float shootRate;
    [SerializeField] int magSizeMax;
    [SerializeField] public float reloadTime;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform invisiGun;

    [SerializeField] AudioClip[] aud;
    [SerializeField] Bullet bulletScript;

    //Public variables
    public List<GunStats> gunList = new List<GunStats>();
    public List<Attachments> attachmentList = new List<Attachments>();
    public int currentAmmo;
    public int startingMaxAmmo;
    public static float shootTimer;
    public float volume;

    public float currentSpread;
    public float hipSpread;
    public float adsSpread;

    public float hipX;
    public float hipY;
    public float hipZ;
    public float adsX;
    public float adsY;
    public float adsZ;

    public float adsZoom;

    public Recoil recoil;

    public bool isShotgun;
    public bool isBurst;
    public float burstTime;
    public float rechamberTime;
    public int burstAmount;
    public int pelletAmount;
    public float burstDelay;

    public int maxAttachments;


    // Other Variables
    bool reloading;
    int activeGun;
    int pelletCount;

    bool burstFiring = true;
    bool shotgunFiring;

    bool isADS;

    void Awake()
    {
        if (instance == null)
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

        if (Input.GetButton("Fire1") && shootTimer >= shootRate && currentAmmo > 0 && isShotgun)
        {
            shotgunFiring = true;

            while(shotgunFiring)
            {
                while(pelletCount != pelletAmount)
                {
                    ShotgunShot();
                }
                StartCoroutine(BurstPellet());              
                shotgunFiring = false;
            }

        }
        else if (Input.GetButton("Fire1") && shootTimer >= shootRate && currentAmmo > 0 && isBurst && burstFiring)
        {
            StartCoroutine(Burst());
        }
        else if (Input.GetButton("Fire1") && shootTimer >= shootRate && currentAmmo > 0 && !isBurst && !isShotgun)
        {
            Shoot();
        }

        /*  Checks to see if currentAmmo is less than or equal to 0 and if the player is not reloading.
            If so, it calls the Reload() method(function) */
        if (currentAmmo <= 0 && !reloading)
        {
            StartCoroutine(Reload());
        }

        if(Input.GetButton("Fire2"))
        {
            ADS();
        }
        else
        {
            HipFire();
        }


        if (Input.GetButton("Reload") && !reloading)
        {
            StartCoroutine(Reload());
        }

    }
    public void callAmmo()
    {
        if (gunList.Count > 0)
        {
            GameManager.instance.Ammocount(currentAmmo, magSizeMax);
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
        if (gunList.Count > 0)
        {
            activeGun = gunPos;
            currentAmmo = gunList[gunPos].magSizeMax;
            magSizeMax = gunList[gunPos].magSizeMax;
            bulletScript = gunList[gunPos].bullet;
            shootRate = gunList[gunPos].shootRate;
            reloadTime = gunList[gunPos].reloadTime;
            aud = gunList[gunPos].aud;
            volume = gunList[gunPos].shotSoundVol;

            attachmentList = gunList[gunPos].attachments;
            maxAttachments = gunList[gunPos].maxAttachments;

            hipSpread = gunList[gunPos].hipSpread;
            adsSpread = gunList[gunPos].adsSpread;

            hipX = gunList[gunPos].hipX;
            hipY = gunList[gunPos].hipY;
            hipZ = gunList[gunPos].hipZ;
            adsX = gunList[gunPos].adsX;
            adsY = gunList[gunPos].adsY;
            adsZ = gunList[gunPos].adsZ;

            adsZoom = gunList[gunPos].adsZoom;

            isShotgun = gunList[gunPos].isShotgun;
            isBurst = gunList[gunPos].isBurst;
            burstTime = gunList[gunPos].burstTime;
            rechamberTime = gunList[gunPos].rechamberTime;
            burstAmount = gunList[gunPos].burstAmount;
            pelletAmount = gunList[gunPos].pelletAmount;
            burstDelay = gunList[gunPos].burstDelay;

            recoil.UpdateRecoil(gunList[gunPos].recoil);
            Destroy(gunModel);
            gunModel = Instantiate(gunList[gunPos].gunModel, invisiGun);
            gunModel.transform.localScale = gunList[gunPos].scale;
            gunModel.transform.localPosition = gunList[gunPos].position;
            gunModel.transform.localRotation = gunList[gunPos].rotation;
            shootPos = gunModel.transform.GetChild(0);
            changeBullet();
            callAmmo();
        }
    }
    public void addAttachment(Attachments attach)
    {


    }
    public void Shoot()
    {
        /*  Checks to see if the player is not reloading. If they are not, it fires a projectile
            and decreases current ammo by 1 */

        if (!reloading)
        {
            shootTimer = 0;
            if (aud[0] != null) 
            GameManager.instance.playerScript.PlayAudio(aud[0], volume);


            Quaternion spreadRotation = shootPos.transform.rotation *
                Quaternion.Euler(Random.Range(-currentSpread, currentSpread), Random.Range(-currentSpread, currentSpread), 0);


            Instantiate(bullet, shootPos.position, spreadRotation);


            currentAmmo = currentAmmo - 1;
            callAmmo();

            recoil.RecoilFire();
        }


    }

    public void ShotgunShot()
    {
        /*  Checks to see if the player is not reloading. If they are not, it fires a projectile
            and decreases current ammo by 1 */

        if (!reloading)
        {
            shootTimer = 0;
            GameManager.instance.playerScript.PlayAudio(aud[0], volume);


            Quaternion spreadRotation = shootPos.transform.rotation *
                Quaternion.Euler(Random.Range(-currentSpread, currentSpread), Random.Range(-currentSpread, currentSpread), 0);


            Instantiate(bullet, shootPos.position, spreadRotation);


            currentAmmo = currentAmmo - 1;
            callAmmo();

            recoil.RecoilFire();
            pelletCount++;
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

    IEnumerator Burst()
    {
       burstFiring = false;

        for (int i = 0; i < burstAmount; i++)
        {
           Shoot();
           yield return new WaitForSeconds(burstTime);
        }

       yield return new WaitForSeconds(burstDelay);
       burstFiring = true;
    }

    IEnumerator BurstPellet()
    {
        yield return new WaitForSeconds(rechamberTime);
        pelletCount = 0;
    }

    public GunStats Swap()
    {
        return gunList[activeGun];
    }

    public void GunListSwap(List<GunStats> listSwap)
    {
        gunList.Clear();
        foreach(GunStats gun in listSwap)
        {
            gunList.Add(gun);
        }
        changeGun(0);
    }

    public void HipFire()
    {
        isADS = false;
        currentSpread = hipSpread;

        recoil.recoil.X = hipX;
        recoil.recoil.Y = hipY;
        recoil.recoil.Z = hipZ;


        Camera.main.fieldOfView = 75;
    }
    public void ADS()
    {
        isADS = true;
        currentSpread = adsSpread;

        recoil.recoil.X = adsX;
        recoil.recoil.Y = adsY;
        recoil.recoil.Z = adsZ;

        Camera.main.fieldOfView = 75 - adsZoom;
    }
}
