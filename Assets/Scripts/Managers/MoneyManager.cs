using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static Action<int> OnMoneyChange;
    private int _money;
    public static MoneyManager instance;

    public int Money
    {
        set
        {
            _money = value;
            OnMoneyChange?.Invoke(_money);
        }
        get
        {
            return _money;
        }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        Money = 100;
    }

    public void AddMoney(int newMoney)
    {
        Money += newMoney;
    }

    public bool CheckIsHaveMoney(int moneyForCheck)
    {
        if (moneyForCheck <= Money) return true;
        return false;
    }

    public void SpendMoney(int newMoney)
    {
        if (CheckIsHaveMoney(newMoney)) Money -= newMoney;
    }

}
