using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<EnemySpawn> enemyPrefabs;
    [SerializeField] public List<GameObject> enemies;
    [SerializeField] [Range(1, 3)] private float minEnemiesSpeed, maxEnemiesSpeed;

    [SerializeField] private TMP_Text waveText;

    // wave works as a multiplier for enemies
    public int wave = 0;

    [SerializeField] private TMP_Text enemiesKilledText;
    public int enemiesKilled = 0;

    [SerializeField] private int defaultWaveEnemiesInt;
    private int remainingSpawns;

    private bool isTutorialOn = true;
    private bool playedIntro = false;

    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialCanvas;

    const float screen_x = 8.5f;
    const float screen_y = 4.5f;

    public static UnityEvent<int> waveUpdate = new UnityEvent<int>();

    private void OnDestroy()
    {
        waveUpdate.RemoveAllListeners();
    }

    private void Start()
    {
        waveUpdate.Invoke(1);
        wave = 1;
        waveText.text = "Wave " + wave;
        if(!playedIntro)
            StartCoroutine(ThemeSongCoroutine());
        tutorialCanvas.SetActive(true);
    }

    IEnumerator ThemeSongCoroutine()
    {
        yield return new WaitForSeconds(AudioManager.instance.Play("Theme start"));
        playedIntro = true;
        AudioManager.instance.Play("Theme");
    }

    private GameObject SpawnEnemy()
    {
        var enemy = Instantiate(enemyPrefabs.GetRandomEnemy(wave));

        Vector2 pos = new Vector2(Random.Range(-11, 11), Random.Range(-6, 6));
        if (Random.Range(0, 2) == 0)
            pos = new Vector2(pos.x, Random.Range(0, 2) == 0 ? screen_y : -screen_y);
        else
            pos = new Vector2(Random.Range(0, 2) == 0 ? screen_x : -screen_x, pos.y);

        enemy.transform.position = pos;

        enemy.transform.parent = transform;
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

    private void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            enemies.Add(SpawnEnemy());
        }
    }

    private void NewWave()
    {
        if (wave == 0 || wave == 1)
            enemiesKilled = 0;

        wave++;
        waveUpdate.Invoke(wave);
        remainingSpawns = Mathf.FloorToInt(wave * defaultWaveEnemiesInt / 2.2f);
        SpawnEnemies();
        waveText.text = "Wave " + wave;
    }

    private void Update()
    {
        if(isTutorialOn && Input.anyKeyDown)
        {
            tutorialCanvas.SetActive(false);
            isTutorialOn = false;
            SpawnEnemies(1);
        }
        enemiesKilledText.text = "You killed: " + enemiesKilled;
        if (enemies.Count <= 0 && !isTutorialOn)
            NewWave();
    }
}

[System.Serializable]
public class EnemySpawn
{
    //USE THIS LATER TO DETERMINE WHERE ENEMIES FIRST START SPAWNING!!!
    public GameObject enemy;
    public int firstWave;
}

public static class GameExtensions
{
    public static GameObject GetRandomEnemy(this List<EnemySpawn> r, int wave)
    {
        List<EnemySpawn> possible = r.FindAll(x => x.firstWave <= wave);
        return possible[Random.Range(0, possible.Count)].enemy;
    }

    public static List<ScrewDriver> GetUnlockedDrivers(this List<ScrewDriver> r, int wave)
    {
        List<ScrewDriver> possible = r.FindAll(x => x.waveUnlock <= wave);
        return possible;
    }
}