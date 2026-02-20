using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    [SerializeField] Transform focusedObject;
    [SerializeField] GameObject prefabFloatingUi;
    [SerializeField] GameObject prefabEnemyHealth;

    private GameObject obj;

    public Transform interactorSorce;
    public float interactRange;

    public float camRotX;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (interactorSorce == null)
            interactorSorce = transform;

    }

    void Update()
    {
        Debug.DrawRay(interactorSorce.position, interactorSorce.forward * interactRange, Color.red);
        mouseControl();
        InteractButton();
        DetectFocusedObject();
        make_FLoatingUi();
    }

    void mouseControl()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sens * Time.deltaTime;
        if (invertY)
        {
            camRotX += mouseY;
        }
        else
        {
            camRotX -= mouseY;
        }
        camRotX = Mathf.Clamp(camRotX, lockVertMin, lockVertMax);
        transform.localRotation = Quaternion.Euler(camRotX, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    void InteractButton()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Ray R = new Ray(interactorSorce.position, interactorSorce.forward);
            if (Physics.Raycast(R, out RaycastHit hitInf, interactRange))
            {
                if (hitInf.collider.gameObject.TryGetComponent(out iInteract interactObj))
                {
                    interactObj.Interacted();

                }
            }
        }
    }
    void DetectFocusedObject()
    {
        Ray ray = new Ray(interactorSorce.position, interactorSorce.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.TryGetComponent(out iInteract _) || hit.collider.gameObject.CompareTag("Door") 
                || hit.collider.gameObject.CompareTag("Objective") || hit.collider.gameObject.CompareTag("LevelDoor"))
                focusedObject = hit.collider.transform;
            else
            {
                focusedObject = null;

            }

        }
        else
        {
            focusedObject = null;
  
        }
    }

    void make_FLoatingUi()
    {

        
        
        if (focusedObject != null)
        {
            if (obj == null)
            {
               Vector3 offset = new Vector3(0, .5f, 0);

                Vector3 dirtoplayer = (GameManager.instance.player.transform.position - focusedObject.position).normalized;
                dirtoplayer.y = 0f;
                float awayset = .5f;
                Vector3 finalPos = focusedObject.position + offset + dirtoplayer * awayset;


                obj = Instantiate(prefabFloatingUi, finalPos, Quaternion.identity);

            }

            prefabFacePlayer(obj);
        }
        else
        {
            if (obj != null)
                Destroy(obj);
        }
    }

    private void prefabFacePlayer(GameObject obj)
    {
        Vector3 playDir = GameManager.instance.player.transform.position - transform.position;

        playDir.y = 0;

        Quaternion rot = Quaternion.LookRotation(playDir);
        obj.transform.rotation = Quaternion.Slerp(transform.rotation, rot, 2f * Time.deltaTime);
    }


}
