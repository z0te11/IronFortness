using UnityEngine;

public class DataPosition : MonoBehaviour
{
    [SerializeField] private Transform[] _positionsForSpawn;

    public static DataPosition instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    public Transform GetTransformForSpawn()
    {
        return _positionsForSpawn[Random.Range(0, _positionsForSpawn.Length)];
    }
}
