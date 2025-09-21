using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldObj : MonoBehaviour
{ 
    [SerializeField] private float timer = 5f;
    void Start()
    {       
       Destroy(gameObject, timer);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            other.gameObject.SetActive(false);
            PoolManager.Instance.Despawn(other.gameObject);
        }
    }
}
