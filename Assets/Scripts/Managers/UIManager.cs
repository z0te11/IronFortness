using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _loseMenu;

    private void OnEnable()
    {
        GameManager.OnWinEndGame += ShowEndGameMenu;
        PauseManager.OnPauseGame += OpenPauseMenu;
        PauseManager.OnUnPauseGame += ClosePauseMenu;
    }

    private void OnDisable()
    {
        GameManager.OnWinEndGame -= ShowEndGameMenu;
        PauseManager.OnPauseGame -= OpenPauseMenu;
        PauseManager.OnUnPauseGame -= ClosePauseMenu;
    }

    public void OpenPauseMenu()
    {
        _pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        _pauseMenu.SetActive(false);
    }

    public void ShowEndGameMenu(bool isWin)
    {
        _pauseMenu.SetActive(false);
        _winMenu.SetActive(false);
        _loseMenu.SetActive(false);
        if (isWin) _winMenu.SetActive(true);
        else _loseMenu.SetActive(true);
    }
}
