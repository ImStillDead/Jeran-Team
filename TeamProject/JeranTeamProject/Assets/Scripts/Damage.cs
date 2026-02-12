using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    enum damageType { bullet, stationary, DOT }     // enum for damage types
    
    // [SerializeFields] for variables we to edit in Unity
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    [SerializeField] float destroyTime;
    [SerializeField] ParticleSystem hitEffect;

    // other variables
    bool isDamaging;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*  Checks if the damageType is bullet. If so, it sets the velocity of the ridgedBody (bullet) 
            equal to the transform.forward (the direction) times the speed variable. */ 
        if (type == damageType.bullet)
        {
            rb.linearVelocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*  Case for triggers to ignore other triggers*/
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
            StartCoroutine(damageOther(dmg));
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;
        d.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }

}
