using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldPowerUp", menuName = "ScriptableObjects/PowerUpData/Shield")]
public class ShieldSO : CollectableData
{
    [SerializeField] private int amount = 5;
    public override void Use(GameObject obj)
    {
        var life = obj.GetComponent<LifeController>();
        life.RestoreShield(amount);
    }
}
