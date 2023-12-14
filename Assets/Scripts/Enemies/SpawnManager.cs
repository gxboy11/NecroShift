using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] spawnPoints;
    public GameObject[] enemies;

    public int waveCount;
    public int wave;
    public int enemyType;

    public bool spawning;

    private int enemiesSpawned;

    private GameManager gameManager;
    void Start()
    {
        waveCount = 2;
        wave = 1;
        spawning = false;
        enemiesSpawned = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

  
    void Update()
    {
        if(spawning == false && enemiesSpawned == gameManager.defeatedEnemies) {
            StartCoroutine(SpawnWave(waveCount));
        }
    }

    IEnumerator SpawnWave(int waveC)
    {
        spawning = true;

        yield return new WaitForSeconds(4); 

        for(int i = 0; i < waveC; i++)
        {
            SpawnEnemy(wave);
            yield return new WaitForSeconds(2);
        }
        wave += 1;
        waveCount += 2;
        spawning = false;

        yield break;
    }

    void SpawnEnemy(int wave)
    {
        int spawnPos = Random.Range(0, 4);
        if(wave == 1)
        {
            enemyType = 1;
        }
        else if(wave < 4)
        {
            enemyType = Random.Range(0, 2);
        }
        else
        {
            enemyType = Random.Range(0, 3);
        }

        Instantiate(enemies[enemyType], spawnPoints[spawnPos].transform.position, spawnPoints[spawnPos].transform.rotation);
        enemiesSpawned += 1;
    }
}
