using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        if (LevelManager.instance == null)
        {
            instance = this;
        }
    }

    public void GameOver()
    {
        UImanager _ui = GetComponent<UImanager>();
        if (_ui != null)
        {
            _ui.ToggleDeathPanel();
        }
    }
}
