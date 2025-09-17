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
    public List<PermanentBoostObject> permanentBoosts;

    [Header("UI")]
    public Transform shopContainer;
    public GameObject buttonCollectablePrefab;
    public GameObject buttonBoostPrefab;
    public UIinventory UIinventory;

    private HashSet<CollectableData> spawnedItems = new HashSet<CollectableData>();
    private HashSet<PermanentBoostObject> spawnedBoosts = new HashSet<PermanentBoostObject>();

    private void Start()
    {
        
        LoadCollectables();
        LoadBoosts();

 
        BoostManager.Instance.InitializeBoostsFromSave(permanentBoosts);

        CreateShopButtons();
    }

    private void CreateShopButtons()
    {

        foreach (var c in collectables)
        {
            if (c == null || spawnedItems.Contains(c)) continue;
            CreateCollectableButton(c);
            spawnedItems.Add(c);
        }

  
        foreach (var b in permanentBoosts)
        {
            if (b == null || spawnedBoosts.Contains(b)) continue;
            CreateBoostButton(b);
            spawnedBoosts.Add(b);
        }
    }

    private void CreateCollectableButton(CollectableData collectable)
    {
        GameObject buttonObj = Instantiate(buttonCollectablePrefab, shopContainer);
        buttonObj.transform.localScale = Vector3.one;

        Button btn = buttonObj.GetComponent<Button>();
        UIButtonShopItem uiButton = buttonObj.GetComponent<UIButtonShopItem>();

        if (uiButton == null)
        {
            Debug.LogError("Prefab buttonCollectablePrefab non contiene UIButtonShopItem!");
            Destroy(buttonObj);
            return;
        }

        uiButton.SetData(collectable);
        btn.onClick.AddListener(() => BuyCollectable(collectable, uiButton));
    }

    private void CreateBoostButton(PermanentBoostObject boost)
    {
        GameObject buttonObj = Instantiate(buttonBoostPrefab, shopContainer);
        buttonObj.transform.localScale = Vector3.one;

        Button btn = buttonObj.GetComponent<Button>();
        UIButtonShopItem uiButton = buttonObj.GetComponent<UIButtonShopItem>();

        if (uiButton == null)
        {
            Debug.LogError("Prefab buttonBoostPrefab non contiene UIButtonShopItem!");
            Destroy(buttonObj);
            return;
        }

        uiButton.SetDataBoost(boost);
        btn.onClick.AddListener(() => BuyBoost(boost, uiButton));
    }

    public void BuyCollectable(CollectableData collectable, UIButtonShopItem uiButton)
    {
        if (!collectable.shopped && CoinManager.Instance.SpendCoins(collectable.cost))
        {
            collectable.shopped = true;
            SaveCollectables();
            uiButton.SetData(collectable);
        }

        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
        if (playerInventory != null)
        {
            playerInventory.AddItem(collectable);
            UIinventory.RefreshCollectables();
        }
    }

    public void BuyBoost(PermanentBoostObject boost, UIButtonShopItem uiButton)
    {
        if (boost.currentLevel >= boost.maxLevel)
        {
            Debug.Log($"{boost.nome} è già al livello massimo!");
            return;
        }

        if (CoinManager.Instance.SpendCoins(boost.cost))
        {
            boost.GrowLevel();
            uiButton.SetDataBoost(boost);
            SaveBoosts();
            BoostManager.Instance.ApplyBoost(boost);
        }
    }

    private void SaveCollectables()
    {
        SaveData data = SaveManager.Load();
        data.collectables.Clear();

        foreach (var c in collectables)
            data.collectables.Add(c.shopped);

        SaveManager.Save(data);
    }

    private void SaveBoosts()
    {
        SaveData data = SaveManager.Load();
        data.boosts.Clear();

        foreach (var b in permanentBoosts)
        {
            data.boosts.Add(new BoostSaveData
            {
                boostName = b.nome,
                currentLevel = b.currentLevel
            });
        }

        SaveManager.Save(data);
    }

    private void LoadCollectables()
    {
        SaveData data = SaveManager.Load();
        for (int i = 0; i < collectables.Count && i < data.collectables.Count; i++)
        {
            collectables[i].shopped = data.collectables[i];
        }
    }

    private void LoadBoosts()
    {
        SaveData data = SaveManager.Load();
        foreach (var b in permanentBoosts)
        {
            BoostSaveData bData = data.boosts.Find(x => x.boostName == b.nome);
            if (bData != null)
            {
                b.currentLevel = bData.currentLevel;
                b.RestoreBoostState();
            }
        }
    }

}
