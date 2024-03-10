using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] int lifespan;
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifespan);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
            dmg.takeDamage(damage);
        Destroy(gameObject);
    }
}