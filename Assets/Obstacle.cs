using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] protected int damage = 1000;
    protected virtual void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            var life = other.GetComponent<LifeController>();
            if (life != null)
            {
                life.TakeDamage(damage);
       
            }  
            gameObject.SetActive(false);

            PoolManager.Instance.Despawn(gameObject);

        }

    }
}
