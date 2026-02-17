using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

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

        if (Input.GetButtonDown("Interact"))
        {
            Ray R = new Ray(interactorSorce.position, interactorSorce.forward);
            if (Physics.Raycast(R, out RaycastHit hitInf, interactRange))
            {
                if (hitInf.collider.gameObject.TryGetComponent(out iInteract interactObj))
                {
                    interactObj.Interacted();
                    Debug.DrawRay(interactorSorce.position, interactorSorce.forward * interactRange, Color.red);
                }
            }
        }

    }
   
}
