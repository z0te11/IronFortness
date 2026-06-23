using UnityEngine;

public class DataWave : MonoBehaviour
{
    [SerializeField] private Wave[] _waves;

    public static DataWave instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public Wave GetDataWave(int numberWave)
    {
        for (int i = 0; i < _waves.Length; i++)
        {
            if (numberWave == _waves[i].numberWave) return _waves[i];
        }
        Debug.LogWarning("No Wave In Data!");
        return null;
    }
}


