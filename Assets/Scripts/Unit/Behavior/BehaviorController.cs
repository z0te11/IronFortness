using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BehaviorController : MonoBehaviour
{
    [SerializeField] private List<IBehavior> _behaviors;
    [SerializeField] private bool _debugMode = true;
    
    private IBehavior _currentBehavior;
    private float _currentPriority = 0f;
    
    private void Awake()
    {
        if (_behaviors == null || _behaviors.Count == 0)
        {
            _behaviors = GetComponents<IBehavior>().ToList();
        }
    }
    
    private void Update()
    {
        if (_behaviors == null || _behaviors.Count == 0) return;
        
        SelectHighestPriorityBehavior();
        
        // Выполняем текущее поведение
        if (_currentBehavior != null)
        {
            _currentBehavior.Realize();
        }
    }
    
    private void SelectHighestPriorityBehavior()
    {
        IBehavior highestBehavior = null;
        float highestPriority = -1f;
        
        foreach (IBehavior behavior in _behaviors)
        {
            if (behavior == null) continue;
            
            float priority = behavior.Behavior();
            
            if (priority > highestPriority)
            {
                highestPriority = priority;
                highestBehavior = behavior;
            }
        }
        
        // Если приоритет изменился
        if (_currentBehavior != highestBehavior)
        {
            _currentBehavior = highestBehavior;
            _currentPriority = highestPriority;
            
            if (_debugMode)
            {
                Debug.Log($"{gameObject.name}: переключился на поведение {_currentBehavior.GetType().Name} с приоритетом {_currentPriority}");
            }
        }
    }
    
}