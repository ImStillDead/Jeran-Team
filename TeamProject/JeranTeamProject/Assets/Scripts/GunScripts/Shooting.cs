using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class Shooting : MonoBehaviour, IAttachmentPickup
{

    public static Shooting instance;

    // [SerializeFields] for variables that we want to edit in Unity
    [SerializeField] public GameObject gunModel;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] float shootRate;
    [SerializeField] int magSizeMax;
    [SerializeField] public float reloadTime;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform invisiGun;
    IKController playerIK;
    [SerializeField] AudioClip[] aud;
    [SerializeField] Bullet bulletScript;

    //Public variables
    public List<GunStats> gunList = new List<GunStats>();
    public List<Attachment> attachmentList = new List<Attachment>();

    public HashSet<AttachmentType> attachmentsOnGun = new HashSet<AttachmentType>();

    private AttachmentType lastscope;
    private AttachmentType lastforegrip;
    private AttachmentType lastmagazine;
    private AttachmentType lastlaser;

    public GameObject scopeOject;
    public GameObject foregripOject;
    public GameObject magazineOject;
    public GameObject laserOject;

    public Transform scopePos;
    public Transform foregripPos;
    public Transform magazinePos;
    public Transform laserPos;


    public AnimationControl animationControl;
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

    public bool isBurst;
    
    public bool canShoot;

    public float burstTime;
    public float rechamberTime;
    public int burstAmount;
    public int pelletAmount;
    public float burstDelay;


    // Other Variables
    bool reloading;
    public int activeGun;
    int pelletCount;
    bool isShotgun;

    float baseAdsSpread;
    float baseHipSpread;
    int baseMagSize;

    bool isADS;

    bool burstFiring = true;
    bool shotgunFiring;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        currentAmmo = magSizeMax;  // Sets currentAmmo equal to the maxAmmo

        canShoot = true;

        recoil = GameObject.Find("CameraRot/CameraRecoil").GetComponent<Recoil>();


    }

    void Start()
    {
        locationFinder();
        animationControl = GameManager.instance.playerScript.playerAnimator;
    }
    // Update is called once per frame
    void Update()
    {
        if(scopePos == null && laserPos == null && foregripPos == null)
        locationFinder();

        shootTimer += Time.deltaTime;

        /*  Gets the input of the fire button and checks if the shoot timer is greater than
            equal to the shoot rate. If it is it calls the Shoot() method(function) */
        if (Input.GetButton("Fire2"))
        {
            ADS();
        }
        else
        {
            HipFire();
        }

        if (Input.GetButton("Fire1") && shootTimer >= shootRate && currentAmmo > 0 && isShotgun)
        {
            shotgunFiring = true;

            while (shotgunFiring)
            {
                while (pelletCount != pelletAmount)
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

        if (Input.GetButton("Reload") && !reloading)
        {
            StartCoroutine(Reload());
        }

    }

    private void locationFinder()
    {
        if(gunModel == null) {  return; }
        Transform[] allChildren = Shooting.instance.gunModel.GetComponentsInChildren<Transform>();

        foreach(Transform part in allChildren)
        {
            if (part.name == "SightPos") scopePos = part;

            if(part.name == "ForegripPos") foregripPos = part;

            if(part.name == "LaserPos") laserPos = part;


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
            aud = gunList[gunPos].shotSound;
            volume = gunList[gunPos].shotSoundVol;

            hipSpread = gunList[gunPos].hipSpread;
            adsSpread = gunList[gunPos].adsSpread;

            hipX = gunList[gunPos].hipX;
            hipY = gunList[gunPos].hipY;
            hipZ = gunList[gunPos].hipZ;
            adsX = gunList[gunPos].adsX;
            adsY = gunList[gunPos].adsY;
            adsZ = gunList[gunPos].adsZ;

            adsZoom = gunList[gunPos].adsZoom;

            isBurst = gunList[gunPos].isBurst;

            burstTime = gunList[gunPos].burstTime;
            rechamberTime = gunList[gunPos].rechamberTime;
            burstAmount = gunList[gunPos].burstAmount;
            pelletAmount = gunList[gunPos].pelletAmount;
            burstDelay = gunList[gunPos].burstDelay;

            recoil.UpdateRecoil(gunList[gunPos].recoil);
            //GameManager.instance.playerScript.weaponPos.SetParent(GameManager.instance.player.transform, false);
            Destroy(gunModel);
            gunModel = Instantiate(gunList[gunPos].gunModel, invisiGun);
            gunModel.transform.localScale = gunList[gunPos].scale;
            gunModel.transform.localPosition = gunList[gunPos].postion;
            gunModel.transform.localRotation = gunList[gunPos].rotation;
            //gunList[gunPos].leftHand = gunModel.transform.Find("LeftHandPos");
            //gunList[gunPos].rightHand = gunModel.transform.Find("RightHandPos");
            shootPos = gunModel.transform.GetChild(0);
            changeBullet();
            callAmmo();

            if (GetGunType() == GunType.Shotgun)
            {
                isShotgun = true;
            }
            else
            {
                isShotgun = false;
            }
            playerIK = GameManager.instance.playerScript.playerIK;
            if(playerIK != null)
            {
                SetHandPosition();
            }
        }
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
    public void HipFire()
    {
        isADS = false;
        currentSpread = hipSpread;

        recoil.recoil.X = hipX;
        recoil.recoil.Y = hipY;
        recoil.recoil.Z = hipZ;


        Camera.main.fieldOfView = 75;
        if(playerIK.gunRightHand != null)
        {
            animationControl.isAiming = false;
            GameManager.instance.playerScript.weaponPos.GetComponent<ParentConstraint>().weight = 1f;
        }
    }

    public void ADS()
    {
        isADS = true;
        currentSpread = adsSpread;

        recoil.recoil.X = adsX;
        recoil.recoil.Y = adsY;
        recoil.recoil.Z = adsZ;

        Camera.main.fieldOfView = 75 - adsZoom;
        if (playerIK.gunRightHand != null)
        {
            animationControl.isAiming = true;
            GameManager.instance.playerScript.weaponPos.GetComponent<ParentConstraint>().weight = 0f;
        }
        
    }

    // Called in Update if the currentAmmo is less than or equal to 0 and the player is not reloading
    IEnumerator Reload()
    {
        
        animationControl.isReloading = true;
        reloading = true;                               // Sets reloading to true to stop the player from firing
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);    // Waits for a set amount of time determined by the reloadTime
        currentAmmo = magSizeMax;                   // Sets currentAmmo equal to the max ammo
        callAmmo();
        reloading = false;                              // Sets reloading back to false so the player can shoot again
        animationControl.isReloading = false;
        canShoot = true;
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


    public GunType GetGunType()
    {
        return gunList[activeGun].gunType;
    }
    public void SetHandPosition()
    {
        playerIK.gunLeftHand = gunModel.transform.Find("LeftHandPos");
        playerIK.gunRightHand = gunModel.transform.Find("RightHandPos");
        ConstraintSource constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = playerIK.animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        constraintSource.weight = 1f;
        GameManager.instance.playerScript.weaponPos.GetComponent<ParentConstraint>().SetSource(0, constraintSource);
        GameManager.instance.playerScript.weaponPos.GetComponent<ParentConstraint>().constraintActive = true;
        GameManager.instance.playerScript.weaponPos.GetComponent<ParentConstraint>().weight = 1f;
        //constraintSource.sourceTransform = gunModel.transform.Find("RightHandPos");
        //constraintSource.weight = 1f;
        //GameManager.instance.playerScript.rightHand.GetComponent<ParentConstraint>().SetSource(0, constraintSource);
        //GameManager.instance.playerScript.rightHand.GetComponent<ParentConstraint>().constraintActive = true;
        //constraintSource.sourceTransform = gunModel.transform.Find("LeftHandPos");
        //GameManager.instance.playerScript.leftHand.GetComponent<ParentConstraint>().SetSource(0, constraintSource);
        //GameManager.instance.playerScript.leftHand.GetComponent<ParentConstraint>().constraintActive = true;
        //GameManager.instance.playerScript.UpdateAnimations();
    }

    public float getShootTimer()
    {
        return shootTimer;
    }

    public void AttachmentOnGun(Attachments accessory)
    {
        // Remove existing of same type
        attachmentsOnGun.Remove(accessory.attachmentType);

        attachmentsOnGun.Add(accessory.attachmentType);
        accessory.isEquipped = true;

        switch (accessory.attachmentType)
        {
            case AttachmentType.Sights:
                scopeOject = accessory.attachmentModel;
                accessory.position = scopePos;
                break;

            case AttachmentType.Foregrips:
                foregripOject = accessory.attachmentModel;
                accessory.position = foregripPos;
                break;

            case AttachmentType.Laser:
                laserOject = accessory.attachmentModel;
                accessory.position = laserPos;
                break;

            case AttachmentType.Magazines:
                magazineOject = accessory.attachmentModel;
                break;
        }

        addAttachment(accessory);
    }

    public void addAttachment(Attachments attachment)
    {
        switch (attachment.attachmentType)
        {
            case AttachmentType.Sights:
                //if (scopeOject != null)
                //{
                //    Destroy(scopeOject.gameObject);
                //    scopeOject = null;
                //}
                scopeOject = Instantiate(attachment.attachmentModel, scopePos);
                scopeOject.transform.localPosition = Vector3.zero;
                scopeOject.transform.localRotation = Quaternion.identity;
                break;

            case AttachmentType.Foregrips:
                if (foregripOject != null) Destroy(foregripOject);
                foregripOject = Instantiate(attachment.attachmentModel);
                break;

            case AttachmentType.Laser:
                if (laserOject != null) Destroy(laserOject);
                laserOject = Instantiate(attachment.attachmentModel);
                break;

            case AttachmentType.Magazines:
                if (magazineOject != null) Destroy(magazineOject);
                magazineOject = Instantiate(attachment.attachmentModel);
                break;
        }
    }

    public void GetAttachmentsStats(Attachments attachment)
    {

        // Equip visually
        AttachmentOnGun(attachment);

        // Apply stats
        Debug.Log("****************" + adsSpread);
        adsSpread -= attachment.adsMod;
        hipSpread -= attachment.hipSpreadMod;
        Debug.Log("**************Update" + adsSpread);

        recoil.recoil.X += attachment.recoilMod;
        recoil.recoil.Y -= attachment.recoilMod;

        magSizeMax += (int)attachment.ammoCountMod;

        // clamp values so they don�t break
        adsSpread = Mathf.Clamp(adsSpread, 0.1f, 100f);
        hipSpread = Mathf.Clamp(hipSpread, 0.1f, 100f);


    }



}
