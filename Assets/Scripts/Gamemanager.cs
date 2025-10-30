using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float MaxHP = 100;
    private float CurrentHP;
    private float MaxEnergy = 100;
    private float CurrentEnergy;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {

        // Ensure only one instance of Gamemanager exists
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    private void Start()
    {
        CurrentHP = MaxHP;
        CurrentEnergy = MaxEnergy;
    }

    public float GetCurrentHP()
    {
        return CurrentHP;
    }
    public float GetMCurrentEnergy()
    {
        return CurrentEnergy;
    }
    public float GetMaxHp()
    {
        return MaxHP;
    }
    public float GetMaxEnergy()
    {
        return MaxEnergy;
    }
    public void ChangeHp(float amout)
    {
        amout = Mathf.Clamp(CurrentHP + amout, 0, MaxHP);
    }
    public void ChangeEnergy(float amout) 
    {
        amout = Mathf.Clamp(CurrentEnergy + amout, 0, MaxEnergy);
    }
}
