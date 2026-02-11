 using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    enum damageType { bullet, stationary, DOT }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] ParticleSystem hitEffect;

    bool isDamaging;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == damageType.bullet)
        {
            rb.linearVelocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && type != damageType.DOT)
        {
            dmg.takeDamage(damageAmount);
        }

        if (type == damageType.bullet)
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && type == damageType.DOT && !isDamaging)
        {
            StartCoroutine(damagOther(dmg));
        }
    }

    IEnumerator damagOther(IDamage D)
    {
        isDamaging = true;
        D.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }

}
