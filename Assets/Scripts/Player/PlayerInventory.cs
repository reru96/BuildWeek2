using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Singleton<PlayerInventory>
{

    private List<CollectableData> collectedItems = new List<CollectableData>();
    private List<PermanentBoostObject> collectedBoosts = new List<PermanentBoostObject>();

    protected override bool ShouldBeDestroyOnLoad() => true;

    public void AddItem(CollectableData item)
    {
        if (item == null || collectedItems.Contains(item)) return;
        collectedItems.Add(item);
    }

    public void AddBoost(PermanentBoostObject boost)
    {
        if (boost == null || collectedBoosts.Contains(boost)) return;

        collectedBoosts.Add(boost);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            boost.PassiveEffect(player);
    }

    public void RestoreBoosts(List<PermanentBoostObject> allBoosts)
    {
        collectedBoosts.Clear();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        foreach (var boost in allBoosts)
        {
            if (boost.currentLevel > 0)
            {
                collectedBoosts.Add(boost);
                boost.PassiveEffect(player);
            }
        }
    }

    public List<CollectableData> GetCollectedItems() => collectedItems;
    public List<PermanentBoostObject> GetCollectedBoosts() => collectedBoosts;
}
