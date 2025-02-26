using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float timeForFirstSpawn = 0;

    public List<Wave> waves;
    public List<SpawnPoint> spawnPoints;
    public int _currentWaveIndex = 0;
    public int _currentSpawnIndex = 0;

    void Start()
    {
        Invoke(nameof(StartSpawn), timeForFirstSpawn);
    }

    void StartSpawn()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator<YieldInstruction> SpawnWave()
    {
        if (_currentWaveIndex < waves.Count)
        {
            Wave wave = waves[_currentWaveIndex];
            for (int i = 0; i < wave.Count; i++)
            {
                SpawnEnemy(wave.EnemyData.EnemyPrefab);
                yield return new WaitForSeconds(wave.TimeBetweenSpawns);
            }
            _currentWaveIndex++;
        }

        //
        Debug.Log("Wave completa");
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (_currentSpawnIndex > spawnPoints.Count)
        {
            _currentSpawnIndex = 0;
        }

        // Spawn
        Transform spawnPoint = spawnPoints[_currentSpawnIndex].SpawnPointPosition;
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // TODO: 


        //
        _currentSpawnIndex++;
    }

    void OnDrawGizmos()
    {
        if (spawnPoints == null)
            return;

        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint != null && spawnPoint.SpawnPointPosition != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(spawnPoint.SpawnPointPosition.position, 0.5f);
                Gizmos.color = Color.white;
            }

            if (spawnPoint != null && spawnPoint.Waypoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(spawnPoint.SpawnPointPosition.position, spawnPoint.Waypoint.transform.position);
                Gizmos.color = Color.white;
            }
        }
    }
}
