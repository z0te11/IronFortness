using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private bool _isStartGame = false;

    private void Start()
    {
        if (_isStartGame) StartGame();
    }
    public void StartGame()
    {
        PauseManager.instance.PauseGame(false);
        if (_levelManager != null) _levelManager.StartLevel(1);
    }

}
