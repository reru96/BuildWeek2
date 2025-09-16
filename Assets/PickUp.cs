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
                PlayerInventory.Instance.AddItem(powerUpData);
                UIinventory ui = FindObjectOfType<UIinventory>();
                if (ui != null)
                    ui.RefreshCollectables();
            }
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}
