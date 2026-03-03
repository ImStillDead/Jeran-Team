using TMPro;
using UnityEngine;

public class Test_objective_collect : MonoBehaviour, iInteract
{

    [SerializeField] string objective1;
    [SerializeField] string dialog;
    GameManager gamemanager;

    void Start()
    {
        gamemanager = GameManager.instance;
    }



    public void Interacted()
    {

        gamemanager.addMission(objective1);

        gamemanager.addDialog(dialog);

        Destroy(gameObject);

    }


}
