using UnityEngine;

public class MainBuilding : MonoBehaviour
{
    public void OnDestroy()
    {
        GameManager.instance.LoseGame();
    }
}
