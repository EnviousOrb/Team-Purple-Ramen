using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int type;//1 = water, 2 = fire, 3 = lightning, 4 = plant
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int effectDuration;
    [SerializeField] float effectStrength;
    [SerializeField] int lifetime;
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        switch (type)
        {
            case 1:
                break;
            case 2:
            IBurn burn = other.GetComponent<IBurn>();
            if (burn != null)
                burn.GetBurnt(effectDuration);
                break;
            case 3:
            IParalyze paralyze = other.GetComponent<IParalyze>();
            if (paralyze != null)
                paralyze.GetParalyzed(effectDuration);
                break;
            case 4:
            ISlow slow = other.GetComponent<ISlow>();
            if (slow != null)
                slow.GetSlowed(effectStrength, effectDuration);
                break;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
            dmg.takeDamage(damage, type);
        Destroy(gameObject);
    }
}
