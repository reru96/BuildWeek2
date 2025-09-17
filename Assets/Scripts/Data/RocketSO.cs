using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldPowerUp", menuName = "ScriptableObjects/PowerUpData/Rocket")]
public class RocketSO : CollectableData
{
    public Vector3 fly = new Vector3(0,2,0);
    public float timer = 5;

    public override void Use(GameObject obj)
    {
        base.Use(obj);

        if (timer > Time.time)
        {
            obj.transform.position += fly;
        }

    }
}
