using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EssenceOrbs : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int healAmount;
    [SerializeField] int manaAmount;
    [SerializeField] int lifespan;
    [SerializeField] int speed;
    void Start()
    {
        agent.stoppingDistance = 0;
        agent.speed = speed;
        Destroy(gameObject, lifespan);
    }
    void Update()
    {
        if (transform.position.y > .5f)
            agent.enabled = false;
        else 
        {
            agent.enabled = true;
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (healAmount > 0)
            {
                IHeal heal = other.GetComponent<IHeal>();
                if (heal != null)
                    heal.GetHealed(healAmount);
            }
            if (manaAmount > 0)
            {
                IMana mana = other.GetComponent<IMana>();
                if (mana != null)
                    mana.GetMana(manaAmount);
            }
            Destroy(gameObject);
        }
    }
}
