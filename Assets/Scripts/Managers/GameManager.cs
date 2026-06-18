using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action<bool> OnWinEndGame;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private bool _isStartGame = false;
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        if (_isStartGame) StartGame();
    }
    public void StartGame()
    {
        PauseManager.instance.PauseGame(false);
        if (_levelManager != null) _levelManager.StartLevel(1);
    }

    public void LoseGame()
    {
        OnWinEndGame?.Invoke(false);
    }

    public void WinGame()
    {
        OnWinEndGame?.Invoke(true);
    }

}
