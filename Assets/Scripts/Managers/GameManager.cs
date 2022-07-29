using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] public List<GameObject> enemies;
    [SerializeField] [Range(1, 3)] private float minEnemiesSpeed, maxEnemiesSpeed;

    public int vawe;

    [SerializeField] private int remainingSpawns; //TODO: remove [SerializeField] 

    private void Start()
    {
        SpawnEnemies();
    }

    private GameObject SpawnEnemy()
    {
        var enemy = Instantiate(enemyPrefab);
        enemy.transform.parent = this.gameObject.transform;
        enemy.transform.position = transform.position;
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
}