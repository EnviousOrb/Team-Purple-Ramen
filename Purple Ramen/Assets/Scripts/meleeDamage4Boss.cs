using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeDamage4Boss : MonoBehaviour
{
    [SerializeField] int meleeStrength;
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.CompareTag("Enemy"))
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(meleeStrength, 0);
        }
    }
}
