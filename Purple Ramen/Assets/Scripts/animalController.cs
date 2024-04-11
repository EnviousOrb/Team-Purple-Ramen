using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class animalController : MonoBehaviour, IDamage
{
    [HeaderAttribute("-----Animal Stats-----")]
    [SerializeField] GameObject itemToPopup;
    [SerializeField] Renderer model;
    [SerializeField] int HP;
    [SerializeField] int speed;

    [HeaderAttribute("-----Animal Roaming-----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int roamDist; 
    [SerializeField] int roamPauseTime;
    [SerializeField] Material damageMat; 

    int originalSpeed;
    bool destinationChosen; 
    float stoppingDistOrig; 
    Vector3 startingPos;
    Material originalMat; 

    void Start()
    {
        originalMat = model.material;
        stoppingDistOrig = agent.stoppingDistance;
        agent.stoppingDistance = 0;
        originalSpeed = speed;
    }

    void Update()
    {
        StartCoroutine(roam());
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randomPos = Random.insideUnitSphere * roamDist + startingPos;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());
        StartCoroutine(RunRandomly());

        if (HP <= 0)
        {
            Destroy(gameObject);
            itemToPopup.SetActive(true);
            StopCoroutine(RunRandomly());
        }
    }

    IEnumerator flashRed()
    {
        model.material = damageMat;
        yield return new WaitForSeconds(0.1f);
        model.material = originalMat; 
    }

    IEnumerator RunRandomly()
    {
        float runTime = 2.0f;
        float startTime = Time.time;

        while (Time.time - startTime < runTime)
        {
            Vector3 randomDirection = new(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
            agent.SetDestination(transform.position + randomDirection * roamDist);
            yield return null;
        }
    }
}
