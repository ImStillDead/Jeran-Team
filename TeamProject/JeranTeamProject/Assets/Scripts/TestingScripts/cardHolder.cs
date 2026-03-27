using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class cardHolder : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    public Transform card1;
    public Transform card2;



    private Guncards cardUI1;
    private Guncards cardUI2;


    PlayerController player;


    private void Start()
    {
        player = GameManager.instance.playerScript;


    }

    private void OnEnable()
    {
        // Grab player reference
        if (player == null)
            player = GameManager.instance.playerScript;

        // Ensure the card prefabs exist
        EnsureCardsExist();

        // Update cards immediately
        updateCards();
    }

    void EnsureCardsExist()
    {
        if (cardUI1 == null && card1 != null)
        {
            GameObject obj1 = Instantiate(prefab, card1);
            obj1.transform.localPosition = Vector3.zero;
            cardUI1 = obj1.GetComponent<Guncards>();
        }

        if (cardUI2 == null && card2 != null)
        {
            GameObject obj2 = Instantiate(prefab, card2);
            obj2.transform.localPosition = Vector3.zero;
            cardUI2 = obj2.GetComponent<Guncards>();
        }
    }


    public void updateCards()
    {
        if (player == null)
        {

            return;
        }

        if (player.Gun == null)
        {
   
            return;
        }

        if (player.holster.gunList == null)
        {

            return;
        }

        var gunlist = player.holster.gunList;



        // ----- CARD 1 -----
        if (cardUI1 != null)
        {
            cardUI1.gameObject.SetActive(false);
        }
        if (gunlist.Count > 0)
        {

            cardUI1.setGunstats(0);
            cardUI1.gameObject.SetActive(true);
        }

        // ----- CARD 2 -----
        if (cardUI2 != null)
        {
            cardUI2.gameObject.SetActive(false); //if there is no object to fill in the list
        }
        if (gunlist.Count > 1)
        {

            cardUI2.setGunstats(1);                          //hopefully update the stats from the gun and put them on the card to read. 
            cardUI2.gameObject.SetActive(true);
        }


    }

    public void Init(PlayerController playercont)
    {
        player = playercont;


        GameObject obj1 = Instantiate(prefab, card1);
        GameObject obj2 = Instantiate(prefab, card2);

        obj1.transform.localPosition = Vector3.zero;
        obj2.transform.localPosition = Vector3.zero ;

        cardUI1 = obj1.GetComponent<Guncards>();
        cardUI2 = obj2.GetComponent<Guncards>();
        
        updateCards();


    }



}
