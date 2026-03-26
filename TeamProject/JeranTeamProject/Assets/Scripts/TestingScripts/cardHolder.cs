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


        if (player == null || player.Gun == null)
        {
            return;
        }


        card1.gameObject.SetActive(false);
        card2.gameObject.SetActive(false);


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
            if (cardUI1 == null)
            {
                GameObject obj1 = Instantiate(prefab, card1);
                obj1.transform.localPosition = Vector3.zero;
                cardUI1 = obj1.GetComponent<Guncards>();
            }

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
            if (cardUI2 == null)
            {
                GameObject obj2 = Instantiate(prefab, card2);// make card at location,? maybe fix static location to be more consistant. 
                obj2.transform.localPosition = Vector3.zero;
                cardUI2 = obj2.GetComponent<Guncards>();
            }

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
