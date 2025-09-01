using UnityEngine;
using System.Collections.Generic;

public class WaveController : MonoBehaviour
{
    [SerializeField]
    private List<EnemySpawner> spawners = new List<EnemySpawner>();
    [SerializeField]
    private float startDelay = 2f;
    [SerializeField]
    private float timeBetweenSpawns = 0.6f;
    [SerializeField]
    private float timeBetweenWaves = 8f;
    [SerializeField]
    private int totalWaves = 5;
    [SerializeField]
    private int baseEnemies = 4;     //enemies in wave 1
    [SerializeField]
    private int enemiesGrowth = 2;  //increase in enemies per wave
    [SerializeField]
    private GameObject bossPrefab;
    [SerializeField]
    private int bossesOnFinalWave = 2;
    [SerializeField]
    private float timeBetweenBossSpawns = 0f;
    private int spawnedBossesThisWave = 0;

    private float timer = 0f;
    private int spawnedThisWave = 0;
    private int currentWave = 0;
    private int enemiesThisWave = 0;
    private bool inWave = false;
    private bool started = false;

    void Update()
    {
        timer += Time.deltaTime;

        //delay before first wave
        if (!started)
        {
            if (timer >= startDelay)
            {
                started = true;
                BeginNextWave();
            }
            return;
        }

        if (inWave)
        {
            //spawn waves with delay between each enemy
            if (currentWave < totalWaves)
            {
                if (spawnedThisWave < enemiesThisWave)
                {
                    if (timer >= timeBetweenSpawns)
                    {
                        SpawnAtRandomSpawner();
                        spawnedThisWave++;
                        timer = 0f;
                    }
                }
                else
                {
                    //wave finished
                    inWave = false;
                    timer = 0f;
                }
            }
            else
            {
                //final wave: spawn bosses with delay between each boss
                if (spawnedBossesThisWave < bossesOnFinalWave)
                {
                    if (timer >= timeBetweenBossSpawns)
                    {
                        SpawnBossAtRandomSpawner();
                        spawnedBossesThisWave++;
                        timer = 0f;
                    }
                }
                else
                {
                    //wave finished
                    inWave = false;
                    timer = 0f;
                }
            }
        }
        else
        {
            //wait between waves
            if (timer >= timeBetweenWaves)
            {
                // check if all waves done
                if (currentWave >= totalWaves)
                {
                    //stop update from running
                    enabled = false;
                    return;
                }
                //otherwise begin next wave
                BeginNextWave();
            }
        }
    }


    void BeginNextWave()
    {
        currentWave++;
        spawnedThisWave = 0;
        timer = 0f;

        if (currentWave < totalWaves)
        {
            enemiesThisWave = baseEnemies + enemiesGrowth * (currentWave - 1);
            inWave = true;
        }
        else
        {
            //final wave
            spawnedBossesThisWave = 0;
            inWave = true;
        }
    }

    void SpawnAtRandomSpawner()
    {
        var s = spawners[Random.Range(0, spawners.Count)];
        s.SpawnOnce();
    }
    private void SpawnBossAtRandomSpawner()
    {
        var s = spawners[Random.Range(0, spawners.Count)];
        s.SpawnCustom(bossPrefab);
    }

}
