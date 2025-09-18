using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int amount = 1;
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

    public void OnCollisionEnter(Collision collision)
    {
        
        if(collision.collider.CompareTag("Player"))
        {
            var life = collision.collider.GetComponent<LifeController>();
            if(life != null)
            { 
                life.TakeDamage(amount);

            }
            else
            {
                Debug.Log("LifeController non trovato sul Player");
            }
            
           
        }
       
    }
}
