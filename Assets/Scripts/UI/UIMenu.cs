using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [Header("UI Menu")]
    [SerializeField] private string startScene;
    [SerializeField] private UIScoreManager uiScoreManager;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject shopMenu;   

    [Header("Riferimenti Player")]
    [SerializeField] private LifeController playerLife;

    private bool isGameOver = false;

    void Start()
    {
        shopMenu.SetActive(false);
        leaderboardPanel.SetActive(false);
        playerLife.OnDeath.AddListener(OnGameOver);

    }

    private void OnGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;


        Time.timeScale = 0f;

        uiScoreManager.SaveAndUpdateLeaderboard();


        if (leaderboardPanel != null)
            leaderboardPanel.SetActive(true);
    }


    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToShop()
    {
        shopMenu.SetActive(true);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(startScene);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        shopMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        shopMenu.SetActive(false);
    }
   
}
