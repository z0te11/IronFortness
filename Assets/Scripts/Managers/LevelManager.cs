using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private WaveController _waveController;
    [SerializeField] private SpawnSystem _spawnSystem;

    public static LevelManager instance;

    private bool _isWaveFinish;
    private bool _isEnemyFinish;
    private int _numberWave;

    private void Start()
    {
        _isWaveFinish = false;
        _isEnemyFinish = false;
        _spawnSystem.SpawnHero(DataUnit.instance.GetHero(DataSaver.currentHero));
    }
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public void StartLevel(int numberLevel)
    {
        _numberWave = numberLevel;

        Wave newWave = DataWave.instance.GetDataWave(_numberWave);
        _waveController.StartSpawnWave(newWave);
    }

    public void WaveIsFinished(bool isFinish)
    {
        if (!isFinish) return;
        StartCoroutine(WaitForSeconds(10f, isFinish));
    }

    public IEnumerator WaitForSeconds(float seconds, bool isFinish)
    {
        yield return new WaitForSeconds(seconds);

        _numberWave += 1;
        if (DataWave.instance.GetDataWave(_numberWave) != null)
        {
            StartLevel(_numberWave);
        } 
        _isWaveFinish = isFinish;
        CheckEndLevel();
        
    }

    public void EnemyIsFinished(bool isFinish)
    {
        _isEnemyFinish = isFinish;
        CheckEndLevel();
    }

    public void CheckEndLevel()
    {
        if (_isEnemyFinish && _isWaveFinish) GameManager.instance.WinGame();
    }
}
