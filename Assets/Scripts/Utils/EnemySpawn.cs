using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public float spawnTimer;
        public float spawnInterval;
        public int enemiesPerWave;
        public int spawnedEnemyCount;
    }

    [Header("Spawn Settings")]
    [SerializeField] private List<Wave> waves = new();
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    public int currentWaveIndex = 0;

    private void Update()
    {
        if(PlayerController.instance == null || !PlayerController.instance.gameObject.activeSelf)
            return;

        if(currentWaveIndex >= waves.Count)
        {
            currentWaveIndex = 0;
            return;
        }

        Wave currentWave = waves[currentWaveIndex];
        currentWave.spawnTimer += Time.deltaTime;

        if (currentWave.spawnTimer >= currentWave.spawnInterval)
        {
            currentWave.spawnTimer = 0;
            spawnenemy(currentWave);
        }

        if(currentWave.spawnedEnemyCount >= currentWave.enemiesPerWave)
        {
            PrepareNextWave(currentWave);
        }

    }

    private void PrepareNextWave(Wave wave)
    {
        wave.spawnedEnemyCount = 0;

        if (wave.spawnInterval > 0.3f)
        {
            wave.spawnInterval *= 0.9f;
        }

        currentWaveIndex++;
    }

    private void spawnenemy(Wave wave)
    {

        if (wave.enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab is null in wave " + currentWaveIndex);
            return;
        }

        Instantiate(wave.enemyPrefab, generateRandomSpawn(), transform.rotation);
        wave.spawnedEnemyCount++;
    }

    private Vector2 generateRandomSpawn()
    {

        float x = 0, y = 0;
        bool horizontal = Random.Range(0f, 1f) > 0.5f;

        if (horizontal)
        {
            x = Random.Range(minPos.position.x, maxPos.position.x);
            y = Random.Range(0f, 1f) > 0.5f ? minPos.position.y : maxPos.position.y;
        }
        else
        {
            y = Random.Range(minPos.position.y, maxPos.position.y);
            x = Random.Range(0f, 1f) > 0.5f ? minPos.position.x : maxPos.position.x;
        }

        return new Vector2(x, y);
    }
}
