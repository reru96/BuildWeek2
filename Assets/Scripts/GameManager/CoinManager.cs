using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : Singleton<CoinManager>  
{
    public int coins = 0;
    public TextMeshProUGUI coinText;

    protected override void Awake()
    {
        base.Awake();

        LoadCoins();
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveCoins();
        UpdateUI();
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            SaveCoins();
            UpdateUI();
            return true;
        }
        return false;
    }

    public void SetCoins(int amount)
    {
        coins = Mathf.Max(0, amount);
        SaveCoins();
        UpdateUI();
    }

    public int GetCoins() => coins;


    private void UpdateUI()
    {
        // Se coinText non è assegnato, cerca in scena un oggetto chiamato "CoinText"
        if (coinText == null)
        {
            GameObject go = GameObject.Find("CoinText");
            if (go != null)
                coinText = go.GetComponent<TextMeshProUGUI>();
        }

       
        if (coinText != null)
            coinText.text = ": " + coins;
    }

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
