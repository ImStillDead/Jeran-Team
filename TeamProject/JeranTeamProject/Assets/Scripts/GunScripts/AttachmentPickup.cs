using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class AttachmentPickup : MonoBehaviour, iInteract
{
    [SerializeField] Attachments attachment;
    GameObject attachmentModel;
    private IAttachmentPickup pick = null;
    bool canSwap;
    Shooting gun;
    GUNHolsters holster;

    private void Start()
    {
        gun = Shooting.instance;

        if (attachmentModel == null && attachment != null)
        {
            attachmentModel = Instantiate(attachment.attachmentModel, transform);
            attachmentModel.transform.localPosition = Vector3.zero;
            attachmentModel.transform.localScale *= 3;
        }
    }


    public void Interacted()
    {

        //ApplyAttachment();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerController player = other.GetComponent<PlayerController>();

            Shooting activeGun = player.Gun;


            bool slotOccupied = false;


            switch (attachment.attachmentType)
            {
                case AttachmentType.Sights:
                    slotOccupied = activeGun.currentGun.sight != null;
                    break;
                case AttachmentType.Foregrips:
                    slotOccupied = activeGun.currentGun.foregrip != null;
                    break;
                case AttachmentType.Laser:
                    slotOccupied = activeGun.currentGun.laser != null;
                    break;
                case AttachmentType.Magazines:
                    slotOccupied = activeGun.currentGun.magazine != null;
                    break;
            }

            if(slotOccupied == false)
            {
                ApplyAttachment(player.Gun);

            }
            else
            {
                player.Gun.SwapAttachment(attachment, this);
            }
        }



    }

    private void ApplyAttachment(Shooting target)
    {
        if (target == null) return;


        target.ApplyAttachment(attachment);

        Destroy(gameObject); // always destroy pickup after use
    }

    public void SetAttachment(Attachments newAttachment)
    {
        attachment = newAttachment;

        // Replace visual
        if (attachmentModel != null)
            Destroy(attachmentModel);

        attachmentModel = Instantiate(attachment.attachmentModel, transform);
        attachmentModel.transform.localPosition = Vector3.zero;
        attachmentModel.transform.localScale *= 3;
    }

}
