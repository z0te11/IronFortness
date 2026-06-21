using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerInstance : MonoBehaviour
{
    public static SceneManagerInstance instance { get; private set; }
    
    [Header("Настройки сцен")]
    [SerializeField] private string _mainMenuScene = "MainMenu";
    [SerializeField] private string _gameScene = "GameScene";
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(_mainMenuScene);
    }
    
    public void LoadGameScene()
    {
        SceneManager.LoadScene(_gameScene);
    }
    
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    
    public void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}