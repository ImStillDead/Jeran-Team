using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.instance.playerSpawn.transform.position != transform.position)
        {
            GameManager.instance.playerSpawn.transform.position = transform.position;
            StartCoroutine(showPop());
        }
    }
    IEnumerator showPop()
    {
        GameManager.instance.playerCheckpointPop.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.playerCheckpointPop.SetActive(false);
    }
}