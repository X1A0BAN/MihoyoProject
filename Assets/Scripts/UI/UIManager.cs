using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [Header("界面容器")]
    [SerializeField]private GameObject MainMenu;
    [SerializeField]private GameObject GameMenu;
    [SerializeField]private GameObject StoryMenu;
    [SerializeField]private GameObject PauseMenu;
    [SerializeField]private GameObject OverMenu;
    [SerializeField]private GameObject WinMenu;
    public enum GameState
    {
        MainMenu,
        InGame,
        Pause,
        Story,
        GameOver,
        Win
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetGameState(GameState newState)//界面切换器
    {
        // 禁用所有界面
        SetAllInterfacesActive(false);

        // 激活对应界面
        switch (newState)
        {
            case GameState.MainMenu:
                MainMenu.gameObject.SetActive(true);
                break;
            case GameState.InGame:
                GameMenu.gameObject.SetActive(true);
                break;
            case GameState.GameOver:
                OverMenu.gameObject.SetActive(true);
                break;
            case GameState.Win:
                WinMenu.gameObject.SetActive(true);
                break;
            case GameState.Pause:
                PauseMenu.gameObject.SetActive(true);
                break;
            case GameState.Story:
                StoryMenu.gameObject.SetActive(true);
                break;
        }
    }

    private void SetAllInterfacesActive(bool v)
    {
        MainMenu.gameObject.SetActive(v);
        GameMenu.gameObject.SetActive(v);
        StoryMenu.gameObject.SetActive(v);
        PauseMenu.gameObject.SetActive(v);
        OverMenu.gameObject.SetActive(v);
        WinMenu.gameObject.SetActive(v);
    }
    public string Currentstate()//获取当前界面状态
    {

        {
            if (MainMenu.gameObject.activeSelf)
            {
                return "MainMenu";
            }
            else if (GameMenu.gameObject.activeSelf)
            {
                return "InGame";
            }
            else if (StoryMenu.gameObject.activeSelf)
            {
                return "GameOver";
            }
            else if (WinMenu.gameObject.activeSelf)
            {
                return "Win";
            }
            else if (PauseMenu.gameObject.activeSelf)
            {
                return "Pause";
            }
            else if (StoryMenu.gameObject.activeSelf)
            {
                return "Story";
            }
            else
            {
                return null;
            }
        }
    }
}
