using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cropScript : MonoBehaviour, IDamage
{
    [SerializeField] GameObject itemToPopup;
    int HP = 1;

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            itemToPopup.SetActive(true);
            Destroy(gameObject);
        }
    }
}
