using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Guncards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image card;
    [SerializeField] Image gunImage;
    [SerializeField] GameObject gunObject;
    [SerializeField] Transform gunlocation;
    [SerializeField] Image overlay;
    [SerializeField] RawImage gunpreview;

    [Header("GeneralUI")]
    [SerializeField] TMP_Text gunName;
    [SerializeField] TMP_Text gunDesc;
    [SerializeField] TMP_Text gunRarity;


    [Header("dmg UI")]
    [SerializeField] Image dmgFill;
    [SerializeField] TMP_Text dmgNumber;

    [Header("accuracy UI")]
    [SerializeField] Image accuracyFill;
    [SerializeField] TMP_Text accuracyNumber;

    [Header("firerate ui")]
    [SerializeField] Image firerateFill;
    [SerializeField] TMP_Text firerateNumber;

    [Header("reload ui")]
    [SerializeField] Image reloadspeedFill;
    [SerializeField] TMP_Text reloadspeedNumber;

    [Header("magSize ui")]
    [SerializeField] Image magsizeFill;
    [SerializeField] TMP_Text magsizeNumber;



    GameManager manager;
    PlayerController player;
    Shooting Gun;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameManager.instance;
        if (manager == null) { Debug.LogError("GameManager.instance is null!"); return; }

        player = manager.playerScript;
        if (player == null) { Debug.LogError("playerScript is null!"); return; }

        Gun = player.Gun;
        if (Gun == null) { Debug.LogError("player.Gun is null!"); return; }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        overlay.color = Color.clear;
        Debug.Log("Mouse Enter");

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overlay.color = new Color32(0, 0, 0, 133);
        Debug.Log("Mouse Exit");
    }

    void SetAlpha(float alpha)
    {
        // Change images
        foreach (var img in GetComponentsInChildren<Image>())
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }

        // Change text
        foreach (var text in GetComponentsInChildren<TMP_Text>())
        {
            Color c = text.color;
            c.a = alpha;
            text.color = c;
        }
    }




    public void setGunstats(int gunPos)
    {
        if (Gun == null || Gun.gunList == null) return;

        var guns = player.Gun.gunList;

        if (gunPos < 0 || gunPos >= guns.Count) return;

        GunStats gun = guns[gunPos];

        if (gun == null) return;

        // Destroy old preview
        if (gunObject != null)
        {
            Destroy(gunObject);
        }

        // Spawn new gun model
        gunObject = Instantiate(gun.gunModel, gunlocation);
        gunObject.tag = "GunPreview";
        gunObject.transform.localRotation = Quaternion.identity;
        gunObject.transform.localPosition = Vector3.zero;
        gunObject.transform.localScale = Vector3.one;

        // UI
        gunName.text = gun.name;
        gunRarity.text = gun.gunRarity.ToString();

        int dmgMult = Mathf.RoundToInt(gun.bullet.damageAmount) * 10;
        dmgNumber.text = dmgMult.ToString();
        dmgFill.fillAmount = Mathf.Clamp01(dmgMult / 100);

        int accMult = (int)gun.spread;
        accuracyNumber.text = accMult.ToString();
        accuracyFill.fillAmount = Mathf.Clamp01(accMult / 100);

        int frrMult = (int)gun.spread; 
        firerateNumber.text = frrMult.ToString();
        firerateFill.fillAmount = Mathf.Clamp01(frrMult / 100);

        int reMult = (int)gun.reloadTime * 10;
        reloadspeedNumber.text = reMult.ToString();
        reloadspeedFill.fillAmount = Mathf.Clamp01(reMult / 100);

        magsizeNumber.text = gun.magSizeMax.ToString();
        magsizeFill.fillAmount = Mathf.Clamp01(gun.magSizeMax / 500);
    }


}
