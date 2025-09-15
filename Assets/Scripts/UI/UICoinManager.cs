using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICoinManager : MonoBehaviour
{

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
   
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinsChanged += UpdateUI;
            UpdateUI(CoinManager.Instance.GetCoins()); 
        }
    }

    private void OnDestroy()
    {
     
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinsChanged -= UpdateUI;
        }
    }

    private void UpdateUI(int coins)
    {
        if (coinText != null)
        {
            coinText.text = ": " + coins;
        }
    }
}

