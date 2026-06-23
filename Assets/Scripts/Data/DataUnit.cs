using UnityEngine;

public class DataUnit : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyUnits;
    [SerializeField] private GameObject[] _heroUnits;
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

    public GameObject GetHero(int number)
    {
        if (number >= _heroUnits.Length) return _heroUnits[0];
        return _heroUnits[number];
    }
}
