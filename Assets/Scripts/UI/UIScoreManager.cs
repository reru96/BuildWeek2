using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreManager : MonoBehaviour
{
    public Transform player;           
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI distanceText;
    public float pointsPerMeter = 1f;  
    public float multiplierRate = 0.1f;

    private float startZ;
    private float distanceTravelled;
    private float timeAlive;
    private int score;
    private float multiplier = 1f;

    void Start()
    {
        startZ = player.position.z;
        score = 0;
        distanceTravelled = 0f;
        timeAlive = 0f;
    }

    void Update()
    {
    
        distanceTravelled = player.position.z - startZ;
        if (distanceTravelled < 0) distanceTravelled = 0;

    
        timeAlive += Time.deltaTime;

     
        multiplier = 1f + (timeAlive * multiplierRate);

        
        score = Mathf.FloorToInt(distanceTravelled * pointsPerMeter * multiplier);

     
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}\nMultiplier: x{multiplier:F2}";
        }

        if (distanceText != null)
        {
            distanceText.text = $"Distance: {distanceTravelled}m";
        }
    }

    public int GetScore()
    {
        return score;
    }

    public float GetMultiplier()
    {
        return multiplier;
    }
}
