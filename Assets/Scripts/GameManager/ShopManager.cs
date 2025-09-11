using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Lista oggetti collezionabili")]
    public List<CollectableData> collectables;

    [Header("UI")]
    public Transform shopContainer;     
    public GameObject buttonPrefab;     
    private HashSet<CollectableData> spawnedItems = new HashSet<CollectableData>();

   private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        if (playerInventory == null)
            Debug.LogWarning("ShopManager: non è stato trovato un PlayerInventory in scena!");

        LoadCollectables();
        CreateShopButtons();
    }

    void CreateShopButtons()
    {
        if (collectables.Count == 0)
        {
            Debug.LogWarning("ShopManager: nessun CollectableData nella lista!");
            return;
        }

        foreach (CollectableData collectable in collectables)
        {
            if (collectable == null)
            {
                Debug.LogWarning("ShopManager: trovato CollectableData null nella lista!");
                continue;
            }

            if (spawnedItems.Contains(collectable))
            {
                Debug.LogWarning($"ShopManager: Collectable '{collectable.namePowerUp}' già creato, salto duplicato.");
                continue;
            }

            GameObject buttonObj = Instantiate(buttonPrefab, shopContainer);
            buttonObj.transform.localScale = Vector3.one;

            Button btn = buttonObj.GetComponent<Button>();
            UIButtonShopItem uiButton = buttonObj.GetComponent<UIButtonShopItem>();

            if (uiButton == null)
            {
                Debug.LogError("Prefab buttonPrefab non contiene UIButtonShopItem!");
                Destroy(buttonObj);
                continue;
            }

            uiButton.SetData(collectable);

            btn.onClick.AddListener(() => BuyCollectable(collectable, uiButton));

            spawnedItems.Add(collectable);
        }
    }

    public void BuyCollectable(CollectableData collectable, UIButtonShopItem uiButton)
    {
        if (!collectable.shopped && CoinManager.Instance.SpendCoins(collectable.cost))
        {
            collectable.shopped = true;
            SaveCollectables();
            uiButton.SetData(collectable);
        }

        if (playerInventory != null)
            playerInventory.AddItem(collectable);
    }

    public void SaveCollectables()
    {
        foreach (var c in collectables)
        {
            PlayerPrefs.SetInt("Collectable_" + c.namePowerUp, c.shopped ? 1 : 0);
        }
    }

    public void LoadCollectables()
    {
        foreach (var c in collectables)
        {
            if (c != null)
                c.shopped = PlayerPrefs.GetInt("Collectable_" + c.namePowerUp, 0) == 1;
        }
    }
}
