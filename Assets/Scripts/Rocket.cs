using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Vector3 fly;
    private float duration;
    private float startTime;

    public void Init(Vector3 fly, float duration, float startTime)
    {
        this.fly = fly;
        this.duration = duration;
        this.startTime = startTime;
    }

    void Update()
    {
        if (Time.time - startTime < duration)
        {
            transform.position += fly * Time.deltaTime;
        }
        else
        {
            Destroy(this);
        }
    }
}
