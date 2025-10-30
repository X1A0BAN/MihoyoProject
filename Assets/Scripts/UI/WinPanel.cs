using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.SetGameState(UIManager.GameState.MainMenu);
        }
    }
    private void MainMenuButton()
    {
        UIManager.Instance.SetGameState(UIManager.GameState.MainMenu);
    }
}
