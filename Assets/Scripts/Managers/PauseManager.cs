using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static Action OnPauseGame;
    public static Action OnUnPauseGame;
    public static PauseManager instance;
    private bool _isPause;
    private bool _isCanUsePause = true;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnWinEndGame += EndGame;
    }

    private void OnDisable()
    {
        GameManager.OnWinEndGame -= EndGame;
    }

    public bool GetStatePause()
    {
        return _isPause;
    }

    public void PauseGame(bool isPause)
    {
        if (!_isCanUsePause) return;

        if (isPause)
        {
            Time.timeScale = 0f;
            OnPauseGame?.Invoke();
            _isPause = true;
        } 
        else
        {
            Time.timeScale = 1f;
            OnUnPauseGame?.Invoke();
            _isPause = false;
        } 
    }

    public void EndGame(bool isWin)
    {
        Time.timeScale = 0f;
        _isCanUsePause = false;
    }
}
