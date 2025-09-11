using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : Singleton<CoinManager>  
{
    public int coins = 0;
    public TextMeshProUGUI coinText;

    protected override void Awake()
    {
        base.Awake();
        coins = PlayerPrefs.GetInt("Coins", 0);
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        PlayerPrefs.SetInt("Coins", coins);
        UpdateUI();
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            PlayerPrefs.SetInt("Coins", coins);
            UpdateUI();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = ": " + coins;
    }
}
