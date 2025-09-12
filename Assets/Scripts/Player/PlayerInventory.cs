using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("UI Inventario")]
    public Transform inventoryContainer;    
    public GameObject inventorySlotPrefab;     

    private List<CollectableData> collectedItems = new List<CollectableData>();

    public void AddItem(CollectableData item)
    {
        if (item == null) return;

        collectedItems.Add(item);
        Debug.Log("Aggiunto all'inventario: " + item.namePowerUp);

      
        if (inventorySlotPrefab != null && inventoryContainer != null)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryContainer);
            slot.name = item.namePowerUp;

            Image icon = slot.GetComponent<Image>();
            if (icon != null && item.icon != null)
                icon.sprite = item.icon;

           
            Button btn = slot.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnSlotClicked(item));
            }
        }
    }

    private void OnSlotClicked(CollectableData item)
    {
        Debug.Log("Slot cliccato: " + item.namePowerUp);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            item.Use(player);
        }
        collectedItems.Remove(item);

    }

    public List<CollectableData> GetInventory() => collectedItems;
}
