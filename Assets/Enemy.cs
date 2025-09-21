using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Enemy : Obstacle
{

    [SerializeField] protected int amount = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private float offsetYaw = 1;
   
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        float newY = Mathf.Sin(Time.time * speed) * offsetYaw;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        
        if(collision.CompareTag("Player"))
        {
            var life = collision.GetComponent<LifeController>();
            if(life != null)
            { 
                life.TakeDamage(amount);
                gameObject.SetActive(false);
                PoolManager.Instance.Despawn(gameObject);

            }
              
        }
       
    }
}
