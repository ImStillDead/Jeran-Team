using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class AttachmentPickup : MonoBehaviour, iInteract
{
    [SerializeField] Attachments attachment;
    GameObject attachmentModel;
    private IAttachmentPickup pick = null;
    bool canSwap;

    Shooting gun;


    private void Start()
    {

        gun = Shooting.instance;
    }


    public void Interacted()
    {

        var receiver = gun.GetComponent<IAttachmentPickup>();

        if (receiver != null)
        {
            receiver.GetAttachmentsStats(attachment);
        }

        Destroy(gameObject);

    } //use interact or walk over it diable this on if you dont want it active


    private void OnTriggerEnter(Collider other)
    {
        var receiver = gun.GetComponent<IAttachmentPickup>();

        if (receiver != null)
        {
            receiver.GetAttachmentsStats(attachment);
        }

        Destroy(gameObject);

    }


}
