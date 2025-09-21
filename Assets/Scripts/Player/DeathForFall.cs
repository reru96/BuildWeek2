using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathForFall : MonoBehaviour
{
    [Header("Fall Death Settings")]
    [SerializeField] private float fallDeathHeight = 2f; 

    [Header("Life Controller")]
    [SerializeField] private LifeController lifeController; 

    private float startY;

    private void Start()
    {
        
        startY = transform.position.y;

        
        if (lifeController == null)
            lifeController = GetComponent<LifeController>();
    }

    private void Update()
    {
        CheckFallDeath();
    }

    private void CheckFallDeath()
    {
        if (lifeController == null) return;

     
        if (transform.position.y < startY - fallDeathHeight)
        {
            lifeController.SetHp(0); 
        }
    }
}
