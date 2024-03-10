using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] Renderer model;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());
        // Event game object dead.
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(1.1f);
        model.material.color = Color.white;
    }
}
