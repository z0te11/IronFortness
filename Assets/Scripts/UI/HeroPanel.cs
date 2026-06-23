using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeroPanel : MonoBehaviour
{
    [SerializeField] public AssetHero[] assetHero;
    [SerializeField] private Image _imageHero;
    [SerializeField] private Text _textLevel;
    [SerializeField] private Text _textLives;
    [SerializeField] private Text _textDamage;
    [SerializeField] private Text _textDamageDisance;
    [SerializeField] private Text _textCooldown;

    public void SetHero(int numberHero)
    {
        _imageHero.sprite = assetHero[numberHero].spriteHero;
        _textLevel.text = assetHero[numberHero].charactersHero.levelUnit.ToString();
        _textLives.text = assetHero[numberHero].charactersHero.lives.ToString();
        _textDamage.text = assetHero[numberHero].charactersHero.damage.ToString();
        _textDamageDisance.text = assetHero[numberHero].charactersHero.damageDisance.ToString();
        _textCooldown.text = assetHero[numberHero].charactersHero.cooldown.ToString();
    }
}
