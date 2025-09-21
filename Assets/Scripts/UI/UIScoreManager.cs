using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreManager : MonoBehaviour
{
    [Header("Riferimenti Player e UI in-game")]
    public Transform player;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI distanceText;

    [Header("Parametri Punteggio")]
    public float pointsPerMeter = 1f;
    public float multiplierRate = 0.1f;

    [Header("Leaderboard")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private List<TMP_Text> scoreTexts;

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

        UpdateLeaderboard();
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
            scoreText.text = $": {score}\n: x{multiplier:F2}";
        }

        if (distanceText != null)
        {
            distanceText.text = $": {distanceTravelled:F0}m";
        }
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        if (scoreText != null)
            scoreText.text = $": {score}\n: x{multiplier:F2}";
    }

   
    public void SaveAndUpdateLeaderboard()
    {
        scoreManager.AddScore(score);

      
        SaveData data = SaveManager.Load();
        data.score = score; 
        SaveManager.Save(data);

        UpdateLeaderboard();
    }

    public void UpdateLeaderboard()
    {
        List<int> scores = scoreManager.GetHighScores();

        for (int i = 0; i < scoreTexts.Count; i++)
        {
            if (i < scores.Count)
            {
                scoreTexts[i].text = $"{i + 1}. {scores[i]}";
            }
            else
            {
                scoreTexts[i].text = $"{i + 1}. ---";
            }
        }
    }

    public int GetScore() => score;
    public float GetMultiplier() => multiplier;
}
