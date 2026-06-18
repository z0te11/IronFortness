using UnityEngine;

public class DataUnit : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyUnits;
    public static DataUnit instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public GameObject GetEnemy(int level)
    {
        for (int i = 0; i < _enemyUnits.Length; i++)
        {
            if (_enemyUnits[i].GetComponent<UnitEnemy>().characters.levelUnit == level) return _enemyUnits[i];
        }
        return _enemyUnits[0];
    }
}
