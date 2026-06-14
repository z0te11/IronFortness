using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private WaveController _waveController;
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
}
