using UnityEngine;

public class MainBuilding : MonoBehaviour
{
    public void Oestroy()
    {
        GameManager.instance.LoseGame();
    }
}
