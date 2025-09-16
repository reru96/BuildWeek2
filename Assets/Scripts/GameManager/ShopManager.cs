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
        LoadCollectables(collectables);
        LoadBoost(permanentBoosts);

        CreateShopButtons();
    }

    private void CreateShopButtons()
    {
        foreach (CollectableData c in collectables)
        {
            if (c == null || spawnedItems.Contains(c)) continue;

            GameObject buttonObj = Instantiate(buttonCollectablePrefab, shopContainer);
            buttonObj.transform.localScale = Vector3.one;

            Button btn = buttonObj.GetComponent<Button>();
            UIButtonShopItem uiButton = buttonObj.GetComponent<UIButtonShopItem>();

            if (uiButton == null)
            {
                Debug.LogError("Prefab buttonPrefab non contiene UIButtonShopItem!");
                Destroy(buttonObj);
                continue;
            }

            uiButton.SetData(c);
            btn.onClick.AddListener(() => BuyCollectable(c, uiButton));

            spawnedItems.Add(c);
        }

        foreach (PermanentBoostObject b in permanentBoosts)
        {
            if (b == null || spawnedBoosts.Contains(b)) continue;

            GameObject buttonObj = Instantiate(buttonBoostPrefab, shopContainer);
            buttonObj.transform.localScale = Vector3.one;

            Button btn = buttonObj.GetComponent<Button>();
            UIButtonShopItem uiButton = buttonObj.GetComponent<UIButtonShopItem>();

            if (uiButton == null)
            {
                Debug.LogError("Prefab buttonPrefab non contiene UIButtonShopItem!");
                Destroy(buttonObj);
                continue;
            }

            uiButton.SetDataBoost(b);
            btn.onClick.AddListener(() => BuyBoost(b, uiButton));

            spawnedBoosts.Add(b);
        }
    }

    public void BuyCollectable(CollectableData collectable, UIButtonShopItem uiButton)
    {
        if (!collectable.shopped && CoinManager.Instance.SpendCoins(collectable.cost))
        {
            collectable.shopped = true;
            SaveCollectables(collectables);
            uiButton.SetData(collectable);
        }

        PlayerInventory.Instance.AddItem(collectable);
        UIinventory.RefreshCollectables();
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
            SaveBoost(permanentBoosts);
        }
       
        PlayerInventory.Instance.AddBoost(boost);
    }

    public void SaveCollectables(List<CollectableData> collectables)
    {
        SaveData data = SaveManager.Load();
        data.collectables.Clear();

        foreach (var c in collectables)
            data.collectables.Add(c.shopped);

        SaveManager.Save(data);
    }

    public void SaveBoost(List<PermanentBoostObject> boosts)
    {

        SaveData data = SaveManager.Load();

        data.boosts.Clear();
        foreach (var b in boosts)
        {
            BoostSaveData bData = new BoostSaveData();
            bData.boostName = b.nome; 
            bData.currentLevel = b.currentLevel;
            data.boosts.Add(bData);
        }

        SaveManager.Save(data);
    }

    public void LoadCollectables(List<CollectableData> collectables)
    {
        SaveData data = SaveManager.Load();

        for (int i = 0; i < collectables.Count && i < data.collectables.Count; i++)
            collectables[i].shopped = data.collectables[i];
    }

    public void LoadBoost(List<PermanentBoostObject> boosts)
    {
        SaveData data = SaveManager.Load();

        foreach (var b in boosts)
        {
            BoostSaveData bData = data.boosts.Find(x => x.boostName == b.nome);
            if (bData != null)
            {
                b.currentLevel = bData.currentLevel;
                b.RestoreBoostState(); 
            }
        }

            PlayerInventory.Instance.RestoreBoosts(boosts);

    }
}
