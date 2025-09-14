using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var life = other.GetComponent<LifeController>();
            {
                life.SetHp(0);
            }
        }
    }
}
