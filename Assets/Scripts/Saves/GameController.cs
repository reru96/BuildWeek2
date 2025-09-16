using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public static SaveData pendingSaveData;

    [SerializeField] private Transform player;

    protected override bool ShouldBeDestroyOnLoad() => false;
    private void Start()
    {
        if (pendingSaveData != null)
        {
           
            if (player != null)
            {
                player.position = new Vector3(
                    pendingSaveData.playerX,
                    pendingSaveData.playerY,
                    pendingSaveData.playerZ
                );
            }

            //ripristina punteggio
            UIScoreManager scoreManager = FindObjectOfType<UIScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.SetScore(pendingSaveData.score); 
            }

            pendingSaveData = null; 
        }
    }
    public void SaveGame(Transform player)
    {
        SaveData data = SaveManager.Load();

        data.sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        data.playerX = player.position.x;
        data.playerY = player.position.y;
        data.playerZ = player.position.z;

        SaveManager.Save(data);
    }

}
