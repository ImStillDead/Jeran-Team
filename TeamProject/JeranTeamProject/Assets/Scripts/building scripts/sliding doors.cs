using UnityEngine;

public class slidingdoores : MonoBehaviour
{

    [Header("Doors")]
    [SerializeField] GameObject rightDoor;
    [SerializeField] GameObject rightParentDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject leftParentDoor;



    [Header("Door settings")]
    [SerializeField] AudioClip doorSound;
    [SerializeField] float doorSpeed;

    [Header("Active door")]
    [SerializeField] bool rightActive;
    [SerializeField] bool leftActive;
    public GameObject wholeRight;
    public GameObject wholeLeft;

    private float doorspeedmult;
    private bool opening;

    Vector3 righMoveDoor;
    Vector3 rightOrigonalPos;
    Vector3 leftMoveDoor;
    Vector3 leftOrigonalPos;

    Vector3 offset;
    Vector3 leftoff;

    private void Start()
    {
        doorspeedmult = 10f * doorSpeed;

        if (rightActive == false)
        {
            wholeRight.SetActive(rightActive);
        }
        if(leftActive == false)
        {
            wholeLeft.SetActive(leftActive);
        }

        offset = new Vector3(.1f, -.1f, 0);
        leftoff = new Vector3(0, 0, -.2f);

        righMoveDoor = rightParentDoor.transform.position;
        rightOrigonalPos = rightDoor.transform.position;

        leftMoveDoor = leftParentDoor.transform.position;
        leftOrigonalPos = leftDoor.transform.position;

    }


    private void Update()
    {

        Doors();

    }

    void Doors()
    {


        if(opening == true)
        {

            if (rightActive == true)

                rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, righMoveDoor + offset, Time.deltaTime * doorspeedmult);


            if (leftActive == true)

                leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftMoveDoor + offset + leftoff, Time.deltaTime * doorspeedmult);
        }
        else
        {

            if (rightActive == true)

                rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightOrigonalPos, Time.deltaTime * doorspeedmult);


            if (leftActive == true)

                leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftOrigonalPos, Time.deltaTime * doorspeedmult);


        }


    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            opening = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            opening = false;
        }
    }


}
