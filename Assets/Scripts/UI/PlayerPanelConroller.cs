using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelConroller : MonoBehaviour
{
    [SerializeField] private Text _moneyText;

    private void OnEnable()
    {
        MoneyManager.OnMoneyChange += ChangeMoneyText;
    }

    private void OnDisable()
    {
        MoneyManager.OnMoneyChange -= ChangeMoneyText;
    }

    private void ChangeMoneyText(int newText)
    {
        _moneyText.text = newText.ToString();
    }
}
