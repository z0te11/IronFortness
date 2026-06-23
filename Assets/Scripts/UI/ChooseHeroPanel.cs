using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseHeroPanel : MonoBehaviour
{
    [SerializeField] private GameObject _heroPanel;
    private HeroPanel _currentPanel;
    private int _currentHero = 0;
    private int _amountHero;

    private void Awake()
    {
        
    }
    public void StartGame()
    {
        DataSaver.currentHero = _currentHero;
        SceneManagerInstance.instance.LoadGameScene();
    }

    public void ChooseHero(int numberHero)
    {
        _currentHero = numberHero;
        var newHeroPanel = Instantiate(_heroPanel, this.transform);
        _currentPanel = newHeroPanel.GetComponent<HeroPanel>();
        _amountHero = _currentPanel.assetHero.Length;
        _currentPanel.SetHero(_currentHero);
    }

    public void NextHero()
    {
        if (_currentHero >= _amountHero - 1) return;
        _currentHero += 1;
        ChooseHero(_currentHero);
    }

    public void PrevHero()
    {
        if (_currentHero <= 0) return;
        _currentHero -= 1;
        ChooseHero(_currentHero);
    }

    public void DestroyPanel()
    {
        Destroy(this.gameObject);
    }
}
