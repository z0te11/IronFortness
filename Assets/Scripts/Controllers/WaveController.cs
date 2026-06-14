using System.Collections;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private SpawnSystem _spawnSystem;
    
    private Wave _currentWave;
    
    public void StartSpawnWave(Wave newWave)
    {
        _currentWave = newWave;
        StartCoroutine(SpawnWaveCoroutine());
    }
    
    private IEnumerator SpawnWaveCoroutine()
    {
        int i = 0;
        Debug.Log($"Волна {_currentWave.numberWave} Начинается!");
        while (i < _currentWave.enemyCount)
        {
            i++;
            yield return new WaitForSeconds(_currentWave.delayBetweenSpawns);
            
            GameObject newUnit = DataUnit.instance.GetEnemy(_currentWave.levelUnit[Random.Range(0, _currentWave.levelUnit.Length)]);
            Transform newPos = DataPosition.instance.GetTransformForSpawn();
            _spawnSystem.SpawnEnemy(newUnit, newPos);
        }

        Debug.Log($"Волна {_currentWave.numberWave} полностью завершена");
    }
    
    
    public void StopWave()
    {
        StopAllCoroutines();
    }
}
