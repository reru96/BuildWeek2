using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldPowerUp", menuName = "ScriptableObjects/PowerUpData/Shield")]
public class ShieldSO : CollectableData
{
    [SerializeField] private int amount = 1;
   
    public override void Use(GameObject obj)
    {
        var life = obj.GetComponent<LifeController>();
        life.AddShield(amount);
        Debug.Log("MaxShield: " + life.GetMaxShield());
        Debug.Log("currentShield: " + life.GetShield());
        base.Use(obj);
    }
}
