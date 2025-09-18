using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIinventory : MonoBehaviour
{
    public Transform inventoryContainer;
    public GameObject inventorySlotPrefab;
    public GameObject player;

    private PlayerInventory playerInventory;

    private void Awake()
    {
        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.OnInventoryChanged += RefreshCollectables;
            }
        }
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= RefreshCollectables;
        }
    }

    public void RefreshCollectables()
    {
        if (playerInventory == null) return;

        foreach (Transform t in inventoryContainer)
            Destroy(t.gameObject);

        foreach (var item in playerInventory.GetCollectedItems())
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryContainer);
            slot.name = item.namePowerUp;

            Image icon = slot.GetComponent<Image>();
            if (icon != null)
                icon.sprite = item.icon;

            Button btn = slot.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => item.Use(player));
            }
        }
    }
}

