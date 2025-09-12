using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public CollectableData powerUpData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (powerUpData != null)
            {
                var player = other.GetComponent<PlayerInventory>();  
                player.AddItem(powerUpData);
            }
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}
