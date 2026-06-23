using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    public static MainUIManager instance;
    [SerializeField] private ChooseHeroPanel _chooseHeroPanel;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void OpenChooseGeroPanel()
    {
        var ChoosePanel = Instantiate(_chooseHeroPanel, this.transform);
        ChoosePanel.GetComponent<ChooseHeroPanel>().ChooseHero(0);
    }
}
    
