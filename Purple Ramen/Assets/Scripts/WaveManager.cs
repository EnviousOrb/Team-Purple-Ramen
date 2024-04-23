using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] spawnLocs;
    public GameObject miniBoss;
    public float timeBetweenWaves = 10f;
    public float spawnRate = 1.0f;

    private int waveNumber = 0;
    private bool waveInProgress;




     void Start()
    {
        startWave();        
    }


    void startWave()
    {
        waveNumber++;
        StartCoroutine(spawnEnemies() );

    }


    IEnumerator spawnEnemies()
    {
        waveInProgress = true;
        int enemiesToSpawn = waveNumber * 2;
        int enemiesSpawned = 0;

        while(enemiesSpawned < enemiesToSpawn) 
        {

            GameObject enemy= enemies[Random.Range(0, enemies.Length)];
            GameObject spawn = spawnLocs[Random.Range(0,spawnLocs.Length)];
            Instantiate(enemy,spawn.transform.position, spawn.transform.rotation );
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnRate);
        }

        yield return new WaitForSeconds(timeBetweenWaves);

        waveInProgress = false;
        if(waveNumber<5 &&waveInProgress==false)
        {
            startWave();
        }
        else if(waveNumber>=5 &&waveInProgress==false) 
            Instantiate(miniBoss, spawnLocs[Random.Range(0, spawnLocs.Length)].transform.position, spawnLocs[Random.Range(0,spawnLocs.Length)].transform.rotation );
    }
}
