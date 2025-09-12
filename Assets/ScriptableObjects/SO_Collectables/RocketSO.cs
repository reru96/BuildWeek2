using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldPowerUp", menuName = "ScriptableObjects/PowerUpData/Rocket")]
public class RocketSO : CollectableData
{
    [SerializeField] private float rocketDistance = 1000f;

    public override void Use(GameObject obj)
    {
        if (obj == null) return;
        Vector3 rocketVector = obj.transform.forward.normalized * rocketDistance + (Vector3.up * 2);
        obj.transform.position += rocketVector;
    }
}
