using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.SetGameState(UIManager.GameState.InGame);
        }
    }

    private void GameButton()
    {
        UIManager.Instance.SetGameState(UIManager.GameState.InGame);
    }
    private void MainMenuButton()
    {
        UIManager.Instance.SetGameState(UIManager.GameState.MainMenu);
    }
    private void QuitButton()
    {
        Application.Quit();
    }
}
