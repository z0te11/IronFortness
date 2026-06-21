using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    public static MainUIManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void StartGame()
    {
        SceneManagerInstance.instance.LoadGameScene();
    }
}
