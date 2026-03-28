using UnityEngine;
using UnityEngine.UI;

public class startMissions : MonoBehaviour, iInteract
{
    [SerializeField] ButtonFunctions loading;
    [SerializeField] GameObject start;
    [SerializeField] Transform startlocation;



    GameManager manager;

    private void Start()
    {
        manager = GameManager.instance;

    }

    public void Interacted()
    {

        loading.LoadRandomScene();



    }

    private void Update()
    {
        manager.guiAlwaysFacePlayerOnPivot(start, startlocation);

    }

}
