using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public CoinData coinData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (coinData != null)
            {

                CoinManager.Instance.AddCoins(coinData.value);
            }
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}
