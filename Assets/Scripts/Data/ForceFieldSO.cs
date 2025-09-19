using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldPowerUp", menuName = "ScriptableObjects/PowerUpData/ForceField")]
public class ForceFieldSO : CollectableData
{
    [SerializeField] private GameObject forceField;

    public override void Use(GameObject obj)
    {
        base.Use(obj);
        
        GameObject field = Instantiate(forceField, obj.transform);

    }
}
