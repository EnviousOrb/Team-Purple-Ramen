using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] spawnLocs;
    public GameObject miniBoss;
    public GameObject areaArena;
    public GameObject areaBoss;
    private int enemiesRemaining;
    public float timeBetweenWaves = 10f;
    public float spawnRate = 1.0f;

    private int waveNumber = 0;
    private bool waveInProgress;




     void Start()
    {
        startWave();
        areaArena = GameObject.FindWithTag("Main Area");
        areaBoss = GameObject.FindWithTag("Miniboss Area");
    }


    void startWave()
    {
        waveNumber++;
        StartCoroutine(spawnEnemies() );
        enemiesRemaining = waveNumber * 2;

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

       
       
       
        if(waveNumber<5 &&waveInProgress==false)
        {
            startWave();
        }
        else if(waveNumber>=5 && waveInProgress == false)
        {
            Instantiate(miniBoss, spawnLocs[Random.Range(0, spawnLocs.Length)].transform.position, spawnLocs[Random.Range(0, spawnLocs.Length)].transform.rotation);
            areaArena.SetActive(false);
            areaBoss.SetActive(true);
        }

    }

    public void enemyDefeated()
    {
        if(!waveInProgress)
        {
            return;

        }

        enemiesRemaining--;

        if(enemiesRemaining == 0)
        {
            waveInProgress=false;
            Debug.Log("Wave " +  waveNumber + " Completed!");
            waveNumber++;
            StartCoroutine(spawnEnemies());
        }

    }

}
