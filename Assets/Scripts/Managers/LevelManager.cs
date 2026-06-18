using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private WaveController _waveController;

    public static LevelManager instance;

    private bool _isWaveFinish;
    private bool _isEnemyFinish;

    private void Start()
    {
        _isWaveFinish = false;
        _isEnemyFinish = false;
    }
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public void StartLevel(int numberLevel)
    {
        switch (numberLevel)
        {
            case 1:
                {
                    Wave newWave = DataWave.instance.GetDataWave(1);
                    _waveController.StartSpawnWave(newWave);
                    break;
                }
        }
    }

    public void WaveIsFinished(bool isFinish)
    {
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
