using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

    private void OnEnable()
    {
        PauseManager.OnPauseGame += OpenPauseMenu;
        PauseManager.OnUnPauseGame += ClosePauseMenu;
    }

    private void OnDisable()
    {
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
}
