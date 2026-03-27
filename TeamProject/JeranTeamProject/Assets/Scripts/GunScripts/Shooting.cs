using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
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
    public GunStats currentGun;
    [SerializeField] GUNHolsters shootingHolster;
    public AnimationControl animationControl;
    public GameObject scopeOject;
    public GameObject foregripOject;
    public GameObject magazineOject;
    public GameObject laserOject;

    public Transform scopePos;
    public Transform foregripPos;
    public Transform magazinePos;
    public Transform laserPos;


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
    IKController playerIK;


    public bool isBurst;

    public float burstTime;
    public float rechamberTime;
    public int burstAmount;
    public int pelletAmount;
    public float burstDelay;


    // Other Variables
    bool reloading;
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
        if (instance == null) instance = this;

        currentAmmo = 0;

        recoil = GameObject.Find("CameraRot/CameraRecoil").GetComponent<Recoil>();

        if (shootingHolster == null)
            shootingHolster = GetComponentInParent<GUNHolsters>(); // if on same object

    }

    void Start()
    {
        animationControl = GameManager.instance.playerScript.playerAnimator;
            locationFinder();

    }


    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;


        if (scopePos == null && laserPos == null && foregripPos == null)
            locationFinder();




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

    public void locationFinder()
    {
        if (Shooting.instance.gunModel != null)
        {
            Transform[] allChildren = Shooting.instance.GetComponentsInChildren<Transform>();

            foreach (Transform part in allChildren)
            {
                if (part.name == "SightPos") scopePos = part;

                if (part.name == "ForegripPos") foregripPos = part;

                if (part.name == "LaserPos") laserPos = part;
            }
        }
    }

    public void callAmmo()
    {
        GameManager.instance.Ammocount(currentAmmo, magSizeMax);
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
    public void Init(GUNHolsters holster)
    {
        shootingHolster = holster;
    }

    public void SetGun(GunStats newGun)
    {
        currentGun = newGun;
        ApplyGunStats(currentGun, true);
        
        
    }
    public void swapGun(GunStats gun)
    {
        currentGun = gun;
        ApplyGunStats(currentGun, false);
      

    }


    void ApplyGunStats(GunStats gun, bool clearAttachments)
    {
        if (clearAttachments)
            ClearAttachments(gun);

        // RESET BASE STATS
        magSizeMax = gun.magSizeMax;
        currentAmmo = magSizeMax;

        bulletScript = gun.bullet;
        shootRate = gun.shootRate;
        reloadTime = gun.reloadTime;

        aud = gun.shotSound;
        volume = gun.shotSoundVol;

        hipSpread = gun.hipSpread;
        adsSpread = gun.adsSpread;

        hipX = gun.hipX;
        hipY = gun.hipY;
        hipZ = gun.hipZ;

        adsX = gun.adsX;
        adsY = gun.adsY;
        adsZ = gun.adsZ;

        adsZoom = gun.adsZoom;

        isBurst = gun.isBurst;

        burstTime = gun.burstTime;
        rechamberTime = gun.rechamberTime;
        burstAmount = gun.burstAmount;
        pelletAmount = gun.pelletAmount;
        burstDelay = gun.burstDelay;

        recoil.UpdateRecoil(gun.recoil);

        isShotgun = gun.gunType == GunType.Shotgun;

        currentGun = gun;

        locationFinder();
        setAttachmentModel();

   

        gunModel.transform.localScale = gun.scale;
        gunModel.transform.localPosition = gun.postion;
        gunModel.transform.localRotation = Quaternion.Euler(0, 180, 0);
        shootPos.transform.localPosition = Vector3.zero + new Vector3(-0.05f, 0, 0f);

        shootingHolster.AddGun(currentGun);
        changeBullet();
        callAmmo();
    }

    void ClearAttachments(GunStats gun)
    {
        if (scopePos != null) foreach (Transform child in scopePos) Destroy(child.gameObject);
        if (laserPos != null) foreach (Transform child in laserPos) Destroy(child.gameObject);
        if (foregripPos != null) foreach (Transform child in foregripPos) Destroy(child.gameObject);
        if (magazinePos != null) foreach (Transform child in magazinePos) Destroy(child.gameObject);

        gun.sight = null;
        gun.laser = null;
        gun.foregrip = null;
        gun.magazine = null;

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

    public Transform GetGunPosition()
    {
        return invisiGun;
    }

    public float getShootTimer()
    {
        return shootTimer;
    }


    void addAttachmenttoObject(Attachments attachment)
    {

        switch (attachment.attachmentType)
        {
            case AttachmentType.Sights:
                scopeOject = attachment.attachmentModel; // i might use this for the gun card, though im not sure if its use
                break;
            case AttachmentType.Foregrips:
                foregripOject = attachment.attachmentModel;
                break;
            case AttachmentType.Laser:
                laserOject = attachment.attachmentModel;
                break;
            case AttachmentType.Magazines:
                magazineOject = attachment.attachmentModel;
                break;
        }


    }

    void setAttachmentModel()
    {
        if (currentGun.sight != null)
            scopeOject = Instantiate(currentGun.sight.attachmentModel, scopePos);

        if (currentGun.laser != null)
            laserOject = Instantiate(currentGun.laser.attachmentModel, laserPos.position, laserPos.rotation, laserPos);

        if (currentGun.foregrip != null)
            foregripOject = Instantiate(currentGun.foregrip.attachmentModel, foregripPos.position, foregripPos.rotation, foregripPos);

        if (currentGun.magazine != null)
            magazineOject = Instantiate(currentGun.magazine.attachmentModel, magazinePos.position, magazinePos.rotation, magazinePos);
    }


    void ResetToBaseStats()
    {
        adsSpread = currentGun.adsSpread;
        hipSpread = currentGun.hipSpread;
        magSizeMax = currentGun.magSizeMax;
        recoil.UpdateRecoil(currentGun.recoil);
    }



    public void ApplyStats(ref float adsSpread, ref float hipSpread, ref Recoil recoil, ref int magSize)
    {
        foreach (var att in currentGun.equippedAttachments)
        {
            adsSpread -= att.adsMod;
            hipSpread -= att.hipSpreadMod;

            recoil.recoil.X += att.recoilMod;
            recoil.recoil.Y += att.recoilMod;

            magSize += (int)att.ammoCountMod;
        }

        adsSpread = Mathf.Clamp(adsSpread, 0.1f, 100f);
        hipSpread = Mathf.Clamp(hipSpread, 0.1f, 100f);
    }

    public void ApplyAttachment(Attachments attachment)
    {
        currentGun.ApplyAttachment(attachment);

        // Recalculate stats from scratch
        ResetToBaseStats();

        ApplyStats(ref adsSpread, ref hipSpread, ref recoil, ref magSizeMax);

        setAttachmentModel();
    }


    public GunType GetGunType()
    {
        return currentGun.gunType;
    }

    public void SwapAttachment(Attachments newAttachment, AttachmentPickup pickup)
    {
        if (currentGun == null) return;

        Attachments oldAttachment = null;

        // FIND OLD ATTACHMENT BY TYPE
        switch (newAttachment.attachmentType)
        {
            case AttachmentType.Sights:
                oldAttachment = currentGun.sight;
                currentGun.sight = newAttachment;
                break;

            case AttachmentType.Foregrips:
                oldAttachment = currentGun.foregrip;
                currentGun.foregrip = newAttachment;
                break;

            case AttachmentType.Laser:
                oldAttachment = currentGun.laser;
                currentGun.laser = newAttachment;
                break;

            case AttachmentType.Magazines:
                oldAttachment = currentGun.magazine;
                currentGun.magazine = newAttachment;
                break;
        }


        RemoveAttachmentModel(newAttachment.attachmentType);

        ApplyAttachment(newAttachment);


        if (pickup != null && oldAttachment != null)
        {
            pickup.SetAttachment(oldAttachment);
        }
    }

    void RemoveAttachmentModel(AttachmentType type)
    {
        switch (type)
        {
            case AttachmentType.Sights:
                if (scopePos != null)
                    foreach (Transform child in scopePos)
                        Destroy(child.gameObject);
                break;

            case AttachmentType.Foregrips:
                if (foregripPos != null)
                    foreach (Transform child in foregripPos)
                        Destroy(child.gameObject);
                break;

            case AttachmentType.Laser:
                if (laserPos != null)
                    foreach (Transform child in laserPos)
                        Destroy(child.gameObject);
                break;

            case AttachmentType.Magazines:
                if (magazinePos != null)
                    foreach (Transform child in magazinePos)
                        Destroy(child.gameObject);
                break;
        }
    }


    public void SetHandPosition()
    {
        if (animationControl == null) { animationControl = GameManager.instance.player.GetComponent<AnimationControl>(); }
        playerIK.gunLeftHand = gunModel.transform.Find("LeftHandPos");
        playerIK.gunRightHand = gunModel.transform.Find("RightHandPos");
        ConstraintSource constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = playerIK.animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        constraintSource.weight = 1f;
        GameManager.instance.playerScript.weaponPos.GetComponent<ParentConstraint>().SetSource(0, constraintSource);
        GameManager.instance.playerScript.weaponPos.GetComponent<ParentConstraint>().constraintActive = true;
        GameManager.instance.playerScript.weaponPos.GetComponent<ParentConstraint>().weight = 1f;

    }
}
