using EchoSphere.Player;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private float originalLength = 0; // 指示条原始宽度
    public Image HPMask;
    public Image EnergyMask;












    private void Start()
    {
        originalLength = HPMask.rectTransform.sizeDelta.x;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButton();
        }

        UpdateHP();
        UpdateEnergy();
    }

    private void UpdateHP()
    {
        HPMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal , GameManager.Instance.GetCurrentHP() / GameManager.Instance.GetMaxHp() * originalLength);
    }
    private void UpdateEnergy()
    {
        EnergyMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GameManager.Instance.GetMCurrentEnergy() / GameManager.Instance.GetMaxEnergy() * originalLength);
    }

    private void PauseButton()
    {
        UIManager.Instance.SetGameState(UIManager.GameState.Pause);
    }

}
