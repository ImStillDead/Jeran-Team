using UnityEngine;

public class AttachmentPickup : MonoBehaviour
{
    [SerializeField] Attachments attachment;
    GameObject attachmentModel;
    private IAttachmentPickup pick = null;
    bool canSwap;

    private void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        pick = other.GetComponent<IAttachmentPickup>();








    }
}
