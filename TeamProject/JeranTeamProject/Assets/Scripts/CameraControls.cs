using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    [SerializeField] Transform focusedObject;
    [SerializeField] Transform focusedEnemy;
    [SerializeField] GameObject prefabFloatingUi;
    [SerializeField] GameObject prefabEnemyHealth;

    public List<GameObject> listofEnemyHealthbars;

    private GameObject obj;
    private GameObject ene;

    public Transform interactorSorce;
    public float enemyViewRange;
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
        make_HealthBar();
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
        Ray Eray = new Ray(interactorSorce.position, interactorSorce.forward);

        focusedObject = null;
        focusedEnemy = null;

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {

            if (hit.collider.TryGetComponent(out iInteract _) || hit.collider.gameObject.CompareTag("Door")
                || hit.collider.gameObject.CompareTag("Objective") 
                || hit.collider.gameObject.CompareTag("LevelDoor"))
            {
                focusedObject = hit.collider.transform;
            }

        }

        if (Physics.Raycast(Eray, out RaycastHit enemyHits, enemyViewRange))
        {
            if (enemyHits.collider.gameObject.CompareTag("Enemy"))
            {
                focusedEnemy = enemyHits.collider.transform;
                
            }

            else
            {
                focusedEnemy = null;
            }

        }
    }

    void make_FLoatingUi()
    {
        if (focusedObject != null)
        {
            if (obj == null)
            {
               Vector3 offset = new Vector3(0, .6f, 0);

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

    void make_HealthBar()
    {            
        Vector3 offset = new Vector3(0, 1.5f, 0);

        if (focusedEnemy != null)
        {

                if (listofEnemyHealthbars.Count <= 0)
                {
                    ene = GameObject.Instantiate(prefabEnemyHealth, focusedEnemy);
                    listofEnemyHealthbars.Add(ene);
                }

                for (int index = 0; index < listofEnemyHealthbars.Count; index++)
                {

                    prefabFacePlayer(listofEnemyHealthbars[index]);
                    listofEnemyHealthbars[index].transform.position = focusedEnemy.position + offset;

            
                    if (focusedEnemy == null || listofEnemyHealthbars[index] == null)
                    {
                        Destroy(listofEnemyHealthbars[index]);
                        listofEnemyHealthbars[index] = null;
                        continue;                
                    }

                }
        }
    }

    private void prefabFacePlayer(GameObject obje)
    {
        Vector3 playDir = GameManager.instance.player.transform.position - transform.position;

        playDir.y = 0;

        Quaternion rot = Quaternion.LookRotation(playDir);
        obje.transform.rotation = Quaternion.Slerp(transform.rotation, rot, 2f * Time.deltaTime);

        if (obje == null) return;

    }


}
