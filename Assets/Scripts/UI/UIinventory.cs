using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIinventory : MonoBehaviour
{
    public Transform inventoryContainer;
    public GameObject inventorySlotPrefab;


    public void RefreshCollectables()
    {
        foreach (Transform t in inventoryContainer)
            Destroy(t.gameObject);

        foreach (var item in PlayerInventory.Instance.GetCollectedItems())
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryContainer);
            slot.name = item.namePowerUp;
            Image icon = slot.GetComponent<Image>();
            if (icon != null) icon.sprite = item.icon;
        }
    }

    public void RefreshBoosts(List<PermanentBoostObject> boosts)
    {
        foreach (Transform t in inventoryContainer)
            Destroy(t.gameObject);

        foreach (var boost in boosts)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryContainer);
            slot.name = boost.nome;
            Image icon = slot.GetComponent<Image>();
            if (icon != null) icon.sprite = boost.icon;
        }
    }
}
