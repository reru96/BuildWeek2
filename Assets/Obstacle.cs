using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] protected int damage = 1000;
    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            var life = collision.collider.GetComponent<LifeController>();
            if (life != null)
            {
                life.TakeDamage(damage);

            }

        }

    }
}
