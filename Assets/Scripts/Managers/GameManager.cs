using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] public List<GameObject> enemies;
    [SerializeField] [Range(1, 3)] private float minEnemiesSpeed, maxEnemiesSpeed;

    [SerializeField] private TMP_Text waveText;

    // wave works as a multiplier for enemies
    public int wave = 0;

    [SerializeField] private int defaultWaveEnemiesInt;
    private int remainingSpawns;

    private void Start()
    {
        SpawnEnemies();
        waveText.text = "Wave " + wave;
    }

    private GameObject SpawnEnemy()
    {
        var enemy = Instantiate(enemyPrefab);
        //enemy.transform.parent = this.gameObject.transform;
        enemy.transform.position = new Vector3(Random.Range(-11, 11), Random.Range(-6, 6), 0);
        //enemy.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        enemy.GetComponent<Enemy>().moveSpeed = Random.Range(minEnemiesSpeed, maxEnemiesSpeed);
        return enemy;
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < remainingSpawns; i++)
        {
            enemies.Add(SpawnEnemy());
        }
    }

    private void NewWave()
    {
        wave++;
        remainingSpawns = (wave * defaultWaveEnemiesInt) / 2;
        SpawnEnemies();
        waveText.text = "Wave " + wave;
    }

    private void Update()
    {
        if (enemies.Count <= 0)
            NewWave();
    }
}