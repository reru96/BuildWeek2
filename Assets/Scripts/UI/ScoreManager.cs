using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private SaveData saveData;

    void Start()
    {
        saveData = SaveManager.Load();
    }

    public void AddScore(int newScore)
    {
        saveData.highScores.Add(newScore);

        // Ordino dal più alto al più basso
        saveData.highScores.Sort((a, b) => b.CompareTo(a));

        // Tengo solo i primi 5
        if (saveData.highScores.Count > 5)
            saveData.highScores = saveData.highScores.GetRange(0, 5);

        SaveManager.Save(saveData);
    }

    public List<int> GetHighScores()
    {
        return saveData.highScores;
    }
}
