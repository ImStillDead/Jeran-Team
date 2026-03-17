using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, iInteract
{
    [SerializeField] GameObject ShopKeeper; 
    [SerializeField] GameObject spawnLocation;
    [SerializeField] GameObject itemForSale;
    [SerializeField] GameObject menu;

    public List<GameObject> shopList;

    GameManager GM;
    PlayerController Pm;


    private GameObject lastitempicked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GM = GameManager.instance;
        Pm = GM.player.GetComponent<PlayerController>();
    }


    public void Interacted()
    {
        GM.menus.openMenuButton(menu);
        

    }


    // Update is called once per frame
    void Update()
    {
        


    }

    public void buyGun(int itemWorth)
    {
        if(itemForSale != null)
        {

            if (Pm.removePlayerMoney(itemWorth) == true)
            {
                Vector3 location = spawnLocation.transform.position;

                GameObject.Instantiate(itemForSale, location, Quaternion.identity.normalized);

            }
        }
        else
        {
            itemForSale = null;
        }


        
    }

    public void ButtonUpdateItem(int selected)
    {
        itemForSale = shopList[selected];

    }

    public void buttonRandomUpdateItem()
    {
        if(itemForSale == lastitempicked || lastitempicked == null)
        {
            itemForSale = shopList[GM.randomNumberPicker(shopList.Count)];
            lastitempicked = null;
            lastitempicked = itemForSale;

        }

    }


}
