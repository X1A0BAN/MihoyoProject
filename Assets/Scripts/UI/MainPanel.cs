using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    private void PlayButton()
    {
        UIManager.Instance.SetGameState(UIManager.GameState.InGame);
    }
    private void QuitButton()
    {
        Application.Quit();
    }
}
