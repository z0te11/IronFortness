using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] public int levelUnit;
    [SerializeField] public float damage;
    [SerializeField] public float damageDisance;
    [SerializeField] public float cooldown;
    private List<IAttack> _iAttacks;

    private void Awake()
    {
        if (_iAttacks == null || _iAttacks.Count == 0)
        {
            _iAttacks = GetComponents<IAttack>().ToList();
        }
    }

    private void Start()
    {
        if (_iAttacks != null)
        {
            foreach (IAttack attack in _iAttacks)
            {
                if (attack == null) continue;
                
                attack.SetAttack(damage);
                attack.SetAttackDistance(damageDisance);
                attack.SetCoolDown(cooldown);
            }
        }
    }
}
