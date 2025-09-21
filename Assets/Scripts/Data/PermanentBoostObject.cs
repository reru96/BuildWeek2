using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentBoostObject : ScriptableObject
{
    public string nome;
    public PermanentBoost boostType;
    public int nextLevel = 1;
    public int currentLevel = 0;
    public int maxLevel = 3;
    public Sprite icon;
    public int cost;
    public bool isShopped = false;
    public int growCost = 2;

    public virtual void GrowLevel()
    {
        currentLevel++;
        nextLevel = currentLevel + 1;
        cost *= growCost;
    }

    public virtual void RestoreBoostState()
    {
       
    }
    public virtual void PassiveEffect(GameObject player)
    {

    }
}
