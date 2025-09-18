using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    private List<CollectableData> collectedItems = new List<CollectableData>();

  
    public event Action OnInventoryChanged;

    public void AddItem(CollectableData item)
    {
        if (item == null || collectedItems.Contains(item)) return;
        collectedItems.Add(item);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(CollectableData item)
    {
        if (item == null) return;
        collectedItems.Remove(item);
        OnInventoryChanged?.Invoke();
    }

    public void ClearCollectables()
    {
        collectedItems.Clear();
        OnInventoryChanged?.Invoke();
    }

    public List<CollectableData> GetCollectedItems() => collectedItems;
}
