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

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public bool GetStatePause()
    {
        return _isPause;
    }

    public void PauseGame(bool isPause)
    {
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
}
