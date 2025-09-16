using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : Singleton<CoinManager>  
{

    public int coins = 0;

    protected override void Awake()
    {
        base.Awake();
        LoadCoins();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveCoins();      
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            SaveCoins();
            return true;
        }
        return false;
    }

    public void SetCoins(int amount)
    {
        coins = Mathf.Max(0, amount);
        SaveCoins();
    }

    public int GetCoins() => coins;


    public event Action<int> OnCoinsChanged;

    private void NotifyChange()
    {
        OnCoinsChanged?.Invoke(coins);
    }

    public void SaveCoins()
    {
        SaveData data = SaveManager.Load();
        data.coins = coins; 
        SaveManager.Save(data);
        NotifyChange();
    }

    public void LoadCoins()
    {
        SaveData data = SaveManager.Load();
        coins = data.coins;
    }
 
}
