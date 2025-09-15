using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : Singleton<CoinManager>  
{

    public int coins = 0;
    public TextMeshProUGUI coinText; // opzionale, se non assegnato lo cerca in scena

    protected override void Awake()
    {
        base.Awake();

        // Carica coins da SaveData JSON
        LoadCoins();
        UpdateUI();
    }

    #region Gestione Coins

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

    #endregion

    #region UI

    private void UpdateUI()
    {
        // Se coinText non è assegnato, prova a trovarlo in scena
        if (coinText == null)
        {
            GameObject go = GameObject.Find("CoinText");
            if (go != null)
                coinText = go.GetComponent<TextMeshProUGUI>();
        }

        // Se esiste, aggiorna il testo
        if (coinText != null)
            coinText.text = ": " + coins;
    }

    #endregion

    #region Eventi

    public event Action<int> OnCoinsChanged;

    private void NotifyChange()
    {
        OnCoinsChanged?.Invoke(coins);
    }

    #endregion

    #region Salvataggio/Caricamento JSON

    public void SaveCoins()
    {
        SaveData data = SaveManager.Load();
        data.coins = coins; // salva coins nel SaveData
        SaveManager.Save(data);
        NotifyChange();
    }

    public void LoadCoins()
    {
        SaveData data = SaveManager.Load();
        coins = data.coins;
    }

    #endregion
}
