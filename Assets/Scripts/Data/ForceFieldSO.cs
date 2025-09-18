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

        
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

       
        GameObject[] allTargets = new GameObject[targets.Length + obstacles.Length];
        targets.CopyTo(allTargets, 0);
        obstacles.CopyTo(allTargets, targets.Length);

        foreach (var target in allTargets)
        {
            if (target == null) continue;

            Vector3 direction = target.transform.position - obj.transform.position;
            float distance = direction.magnitude;

            if (distance <= radius)
            {
                Vector3 pushVector = direction.normalized * forceStrength;
                target.transform.position += pushVector * Time.deltaTime;
            }
        }
    }

}
