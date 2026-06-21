using System.Collections;
using UnityEngine;

public class GeneratorGold : MonoBehaviour
{
    [SerializeField] private int _goldPerTick = 10;
    [SerializeField] private float _tickInterval = 10f;
    
    private Coroutine _goldCoroutine;
    
    private void Start()
    {
        StartGoldGeneration();
    }
    
    private void OnDisable()
    {
        StopGoldGeneration();
    }
    
    public void StartGoldGeneration()
    {
        if (_goldCoroutine != null)
        {
            StopCoroutine(_goldCoroutine);
        }
        _goldCoroutine = StartCoroutine(GenerateGoldCoroutine());
    }
    
    public void StopGoldGeneration()
    {
        if (_goldCoroutine != null)
        {
            StopCoroutine(_goldCoroutine);
            _goldCoroutine = null;
        }
    }
    
    private IEnumerator GenerateGoldCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_tickInterval);
            
            MoneyManager.instance.AddMoney(_goldPerTick);
            Debug.Log($"Сгенерировано {_goldPerTick} золота");

        }
    }
    
    public void SetGoldPerTick(int amount)
    {
        _goldPerTick = amount;
    }
    
    public void SetTickInterval(float interval)
    {
        _tickInterval = interval;
    }
}