using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public static SaveData pendingSaveData;

    protected override bool ShouldBeDestroyOnLoad() => false;
    private void Start()
    {
        if (pendingSaveData != null)
        {

            UIScoreManager scoreManager = FindObjectOfType<UIScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.SetScore(pendingSaveData.score); 
            }

            pendingSaveData = null; 
        }
    }
    public void SaveGame()
    {
        SaveData data = SaveManager.Load();

        data.sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        SaveManager.Save(data);
    }

}
