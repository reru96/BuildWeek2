using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostManager : Singleton<BoostManager>
{
    private List<PermanentBoostObject> activeBoosts = new List<PermanentBoostObject>();

    public void Start()
    {
        Debug.Log("Quanti boost:" + activeBoosts.Count);
    }

    public void InitializeBoostsFromSave(List<PermanentBoostObject> allBoosts)
    {
        activeBoosts.Clear();
        foreach (var boost in allBoosts)
        {
            if (boost.currentLevel > 0)
            {
                activeBoosts.Add(boost);
            }
        }

        ApplyAllBoosts();
    }

    public void ApplyBoost(PermanentBoostObject boost)
    {
        if (boost == null) return;

        if (!activeBoosts.Contains(boost))
            activeBoosts.Add(boost);

        boost.RestoreBoostState();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            boost.PassiveEffect(player);
    }

    public void ApplyAllBoosts()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        foreach (var boost in activeBoosts)
        {
            boost.RestoreBoostState();
            boost.PassiveEffect(player);
        }
    }

    public List<PermanentBoostObject> GetActiveBoosts() => activeBoosts;
}
