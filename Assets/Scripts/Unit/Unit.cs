using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] public Characters characters;
    private List<IAttack> _iAttacks;
    private List<ILives> _iLives;

    protected void Awake()
    {
        if (_iAttacks == null || _iAttacks.Count == 0)
        {
            _iAttacks = GetComponents<IAttack>().ToList();
        }
        if (_iLives == null || _iLives.Count == 0)
        {
            _iLives = GetComponents<ILives>().ToList();
        }
    }

    protected virtual void Start()
    {
        RealizeData();
    }

    public virtual void RealizeData()
    {
        if (_iAttacks != null)
        {
            foreach (IAttack attack in _iAttacks)
            {
                if (attack == null) continue;
                
                attack.SetAttack(characters.damage);
                attack.SetAttackDistance(characters.damageDisance);
                attack.SetCoolDown(characters.cooldown);
            }
        }

        if (_iLives != null)
        {
            foreach (ILives live in _iLives)
            {
                if (live == null) continue;
                
                live.SetLives(characters.lives);

            }
        }
    }
}
