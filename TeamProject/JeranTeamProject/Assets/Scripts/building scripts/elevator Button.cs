using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class elevatorButton : MonoBehaviour, iInteract
{
    public GameObject InButton;
    public Transform buttonParent;
    public liftscript lift;
    public int floor;

    private Vector3 initialPos;
    private Vector3 pressedPos;
    private bool isPressed;


    

    void Start()
    {
        initialPos = InButton.transform.localPosition;
        pressedPos = initialPos + new Vector3(0, -0.05f, 0); 


        if (lift == null)
        {
            lift = GetComponentInParent<liftscript>();
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isPressed)
        {
            InButton.transform.localPosition = Vector3.Lerp(InButton.transform.localPosition, pressedPos, Time.deltaTime * 5);
        }
        else
        {
            InButton.transform.localPosition = Vector3.Lerp(InButton.transform.localPosition, initialPos, Time.deltaTime * 1);
        }

        if (Vector3.Distance(InButton.transform.localPosition, pressedPos) < 0.01f)
        {
            isPressed = false;
        }


    }
    public void Interacted()
    {
        if (lift != null)
        {
            lift.setFloor(floor);
            isPressed = true;


        }
        else
        {
            Debug.LogError("Button pressed but no lift assigned: " + gameObject.name);
        }

    }



}
