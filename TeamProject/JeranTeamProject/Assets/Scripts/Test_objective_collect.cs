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

        gamemanager.obj_text(objective1);

        gamemanager.dialogText(dialog);

    }


}
