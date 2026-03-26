using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Guncards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("card object")]
    [SerializeField] GameObject cardHolder;
    [SerializeField] GameObject card;
    [SerializeField] Image gunImage;
    [SerializeField] GameObject gunObject;
    [SerializeField] Transform gunlocation;
    [SerializeField] Image overlay;
    [SerializeField] RawImage gunpreview;

    [Header("GeneralTextUI")]
    [SerializeField] TMP_Text gunName;
    [SerializeField] TMP_Text gunRarity;


    [Header("dmg UI")]
    [SerializeField] Image dmgFill;
    [SerializeField] TMP_Text dmgNumber;

    [Header("hipFire accuracy UI")]
    [SerializeField] Image HipaccuracyFill;
    [SerializeField] TMP_Text HipaccuracyNumber;

    [Header("ADS accuracy UI")]
    [SerializeField] Image ADSaccuracyFill;
    [SerializeField] TMP_Text ADSaccuracyNumber;

    [Header("firerate ui")]
    [SerializeField] Image firerateFill;
    [SerializeField] TMP_Text firerateNumber;

    [Header("reload ui")]
    [SerializeField] Image reloadspeedFill;
    [SerializeField] TMP_Text reloadspeedNumber;

    [Header("magSize ui")]
    [SerializeField] Image magsizeFill;
    [SerializeField] TMP_Text magsizeNumber;

    [Header("pannel colors")]
    [SerializeField] Image background;
    [SerializeField] Image namePanel;
    [SerializeField] Image rarityPanel;
    [SerializeField] Image statpanel;


    [Header("rawimage unique to gunCard")]
    public RawImage rawImage;
    public Camera prefabCamera;


    GameManager manager;
    PlayerController player;
    Shooting Gun;
    cardHolder holder;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameManager.instance;
        if (manager == null) { Debug.LogError("GameManager.instance is null!"); return; }

        player = manager.playerScript;
        if (player == null) { Debug.LogError("playerScript is null!"); return; }

        Gun = player.Gun;
        if (Gun == null) { Debug.LogError("player.Gun is null!"); return; }

        RenderTexture rt = new RenderTexture(256, 256, 16);
        prefabCamera.targetTexture = rt;
        rawImage.texture = rt;

        card = transform.parent.gameObject;
        cardHolder = card.transform.parent.gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        overlay.color = Color.clear;
        card.transform.SetAsLastSibling();
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

    void setColor(byte Red, byte Green, byte Blue)
    {

        Color32 NColor = new Color32(Red, Green, Blue, 255);
        Color32 BackColor = new Color32((byte)(Red * 0.7), (byte)(Green * 0.7), (byte)(Blue * 0.7), 255);

        background.color = BackColor;
        namePanel.color = NColor;
        rarityPanel.color = NColor;
        statpanel.color = NColor;

    }


    //sight ----laser---foregrip attatchments add


    public void setGunstats(int gunPos)
    {
        if (Gun == null || player.holster.gunList == null) return;

        var guns = player.holster.gunList;

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
        gunObject.layer = LayerMask.NameToLayer("GunPreview");
        gunObject.transform.localRotation = Quaternion.identity;
        gunObject.transform.localPosition = Vector3.zero;
        gunObject.transform.localScale = Vector3.one;

        Debug.Log("set card gameobject to" + gun.gunModel);


        // UI
        gunName.text = gun.name;
        gunRarity.text = gun.gunRarity.ToString();

        int dmgMult = Mathf.RoundToInt(gun.bullet.damageAmount) * 10;
        dmgNumber.text = dmgMult.ToString();
        dmgFill.fillAmount = Mathf.Clamp01(dmgMult / 100f);

        float accHipPercent = gun.hipSpread * 10f;
        HipaccuracyNumber.text = Mathf.RoundToInt(accHipPercent).ToString();
        HipaccuracyFill.fillAmount = Mathf.Clamp01(accHipPercent / 100f);

        float accADSPercent = gun.adsSpread * 10f;
        ADSaccuracyNumber.text = Mathf.RoundToInt(accADSPercent).ToString();
        ADSaccuracyFill.fillAmount = Mathf.Clamp01(accADSPercent / 100f);

        int frrMult = Mathf.RoundToInt(gun.shootRate * Gun.getShootTimer()); 
        firerateNumber.text = frrMult.ToString();
        firerateFill.fillAmount = Mathf.Clamp01(frrMult / 100f);

        int reMult = Mathf.RoundToInt(gun.reloadTime * 10f);
        reloadspeedNumber.text = reMult.ToString();
        reloadspeedFill.fillAmount = Mathf.Clamp01(reMult / 100f);

        magsizeNumber.text = gun.magSizeMax.ToString();
        magsizeFill.fillAmount = Mathf.Clamp01(gun.magSizeMax / 500f);

        Debug.Log("set gun dmg number to" + gun.bullet.damageAmount);
        Debug.Log("set gun hip number to" + gun.hipSpread);
        Debug.Log("set gun ads number to" + gun.adsSpread);
        Debug.Log("set gun firerate number to" + gun.shootRate);
        Debug.Log("set gun reload number to" + gun.reloadTime);
        Debug.Log("set gun magsize number to" + gun.magSizeMax);

        cardColor(gun);
    }

    void cardColor(GunStats color)
    {




        if(color != null && color.gunRarity == GunRarity.Common)
        {
            setColor(155, 155, 155);
        }
        else if(color != null && color.gunRarity == GunRarity.Uncommon)
        {
            setColor(123, 164, 79);
        }
        else if(color != null && color.gunRarity == GunRarity.Rare)
        {
            setColor(80, 137, 164);
        }
        else if(color != null && color.gunRarity == GunRarity.Perfected)
        {
            setColor(224, 174, 34);
        }
        else if(color != null && color.gunRarity == GunRarity.Exotic)
        {
            setColor(226, 62, 48);
        }
        else if(color != null && color.gunRarity == GunRarity.Special)
        {

        }





    }


}
