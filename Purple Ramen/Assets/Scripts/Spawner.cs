using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] Transform[] spawnPOS;
    [SerializeField] int distanceBeforeSpawn;
    [SerializeField] int spawnRate;
    [SerializeField] int maxEnemies;
    int enemiesInScene;
    bool spawnCD;
    bool spawning;

    void Start()
    {

    }
    void Update()
    {
        if (spawning && !spawnCD && enemiesInScene < maxEnemies)
            StartCoroutine(Spawn());
    }
    public void UpdateEnemies(int amount)
    {
        enemiesInScene += amount;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            spawning = true;
    }
    IEnumerator Spawn()
    {
        spawnCD = true;
        int arrayPOS = Random.Range(0, spawnPOS.Length);
        float distance = Vector3.Distance(spawnPOS[arrayPOS].transform.position, gameManager.instance.player.transform.position);
        if (distance > distanceBeforeSpawn)
        {
            GameObject spawnedEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPOS[arrayPOS].transform.position, spawnPOS[arrayPOS].transform.rotation);
            if (spawnedEnemy.GetComponent<enemyAI>())
                spawnedEnemy.GetComponent<enemyAI>().associatedSpawner = this;
            enemiesInScene++;
            yield return new WaitForSeconds(spawnRate);
        }
        spawnCD = false;
    }
}
