using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [Header("recoil ui")]
    [SerializeField] Image recoilFill;
    [SerializeField] TMP_Text recoilNumber;

    [Header("firerate ui")]
    [SerializeField] Image firerateFill;
    [SerializeField] TMP_Text firerateNumber;



    Shooting Gun;
    GameManager manager;
    PlayerController player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameManager.instance;
        if (manager == null) { Debug.LogError("GameManager.instance is null!"); return; }

        player = manager.playerScript;
        if (player == null) { Debug.LogError("playerScript is null!"); return; }

        Gun = player.Gun;
        if (Gun == null) { Debug.LogError("player.Gun is null!"); return; }

        
        SetAlpha(0);
        gunpreview.color = new Color32(0, 0, 0, 0);

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

    public void setGunstats(GunStats gun)
    {
        if (gun == null) return;


        dmgNumber.text = Mathf.RoundToInt(gun.bullet.damageAmount).ToString();
        dmgFill.fillAmount = gun.bullet.damageAmount / 100;

        accuracyNumber.text = gun.spread.ToString();
        accuracyFill.fillAmount = gun.spread / 100;

        recoilNumber.text = gun.recoil.ToString();
        
        firerateNumber.text = gun.shootRate.ToString(); 
        firerateFill.fillAmount = gun.shootRate / 100;

    }


}
