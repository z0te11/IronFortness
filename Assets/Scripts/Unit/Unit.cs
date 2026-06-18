using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] public UnitCharacters characters;
    private List<IAttack> _iAttacks;
    private List<ILives> _iLives;

    private void Awake()
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

    private void Start()
    {
        RealizeData();
    }

    public void RealizeData()
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
