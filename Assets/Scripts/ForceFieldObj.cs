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
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy") || other.collider.CompareTag("Obstacle"))
        {
            other.gameObject.SetActive(false);
            PoolManager.Instance.Despawn(other.gameObject);
        }
    }
}
