using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldPowerUp", menuName = "ScriptableObjects/PowerUpData/Rocket")]
public class RocketSO : CollectableData
{
    public Vector3 fly = new Vector3(0, 10, 0);
    public float duration = 5f; 
    private float startTime;

    public override void Use(GameObject obj)
    {
        base.Use(obj);

        startTime = Time.time; 
        obj.AddComponent<Rocket>().Init(fly, duration, startTime);
    }
}
