using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldPowerUp", menuName = "ScriptableObjects/PowerUpData/ForceField")]
public class ForceFieldSO : CollectableData
{
    [SerializeField] private float forceStrength = 10f;
    [SerializeField] private float radius = 5f;

    public override void Use(GameObject obj)
    {
        base.Use(obj);

        if (obj == null) return;

        Collider[] colliders = Physics.OverlapSphere(obj.transform.position, radius);

        foreach (var col in colliders)
        {
            if (col == null) continue;

        
            if (col.CompareTag("Enemy") || col.CompareTag("Obstacle"))
            {
                col.gameObject.SetActive(false); 
                PoolManager.Instance.Despawn(col.gameObject);
            }

           

        }
    }
}
