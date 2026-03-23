using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class liftscript : MonoBehaviour
{
    [Header("locations of elevator")]
    [SerializeField] Transform parentObject;
    [SerializeField] List<Vector3> targetLocation;
    [SerializeField] TagHandle tags;
    public int floor;

    [Header("doors/objects")]
    [SerializeField] GameObject doorParent;
    [SerializeField] GameObject door;
    [SerializeField] GameObject elevatorBox;

    [Header("settings")]
    [SerializeField] float elevatorSpeed;
    [SerializeField] float elevatorDoorSpeed;
    [SerializeField] TMP_Text floorNumber;
    [SerializeField] AudioSource elevatorspeaker;
    [SerializeField] AudioClip bell;
    [SerializeField] AudioClip elevator;



    Vector3 MoveDoor;
    Vector3 OrigonalPos;
    Vector3 MoveParentDoor;
    Vector3 OrigonalParentPos;

    private Vector3 orgPos;
    private Vector3 target;
    private bool isMoving = false;
    public Vector3 currentPos;
    public float newY;
    private float elevatorspeedMult;
    private float elevatorDoorSpeedMult;
    private bool firstDoorOpen;

    void Start()
    {
        elevatorspeedMult = 5 * elevatorSpeed;
        elevatorDoorSpeedMult = 5 * elevatorDoorSpeed;

        orgPos = parentObject.transform.position; 

        Vector3 offset = new Vector3(-4f,-1f,0f);

        OrigonalPos = door.transform.localPosition;
        OrigonalParentPos = doorParent.transform.localPosition;

        MoveDoor = OrigonalPos + new Vector3(-.05f, 0f, 1.5f); 
        MoveParentDoor = OrigonalParentPos + new Vector3(0f, 0f, 1f);



    }


    void Update()
    {

        doorsOpen();
        moveFloor();
    }

    void moveFloor()
    {
        if (floor != 0 && floor >= targetLocation.Count)
        {
            Debug.Log("your floor choice does not exist");
            return;
        }


        target = targetLocation[floor];
        isMoving = true;

        if (isMoving)
        {
            currentPos = elevatorBox.transform.position;

            newY = Mathf.MoveTowards(currentPos.y, target.y, Time.deltaTime * elevatorspeedMult);

            elevatorBox.transform.position = new Vector3(currentPos.x, newY, currentPos.z);


            if (Mathf.Approximately(elevatorBox.transform.position.y, target.y))
            {
                isMoving = false;


            }
        }
    }

    void doorsOpen()
    {
        if (isMoving == false)
        {
            door.transform.localPosition = Vector3.Lerp(door.transform.localPosition, MoveDoor, Time.deltaTime * elevatorDoorSpeedMult);
            doorParent.transform.localPosition = Vector3.Lerp(doorParent.transform.localPosition, MoveParentDoor, Time.deltaTime * elevatorDoorSpeedMult);
            
            floorNumber.text = floor.ToString();

        }
        if (isMoving == true)
        {
            door.transform.localPosition = Vector3.Lerp(door.transform.localPosition, OrigonalPos, Time.deltaTime * elevatorDoorSpeedMult);
            doorParent.transform.localPosition = Vector3.Lerp(doorParent.transform.localPosition, OrigonalParentPos, Time.deltaTime * elevatorDoorSpeedMult);

            floorNumber.text = floor.ToString();

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isMoving == true)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.SetParent(elevatorBox.transform);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(isMoving == false)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.SetParent(null); 
            }
        }
    }

    public void setFloor(int number)
    {
        if (number >= 0 && number < targetLocation.Count)
        {
            floor = number;
        }
        else
        {
            Debug.LogWarning("Invalid floor number: " + number);
        }
    }

}
