using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] boostscript boostData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player == null) return;
            {
                player.Heal(boostData.healthrefill);
                Destroy(gameObject);
            }
        }
    }
}
