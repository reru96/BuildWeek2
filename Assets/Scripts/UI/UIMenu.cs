using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private string startScene;
    public GameObject menu;
    void Start()
    {
        menu.SetActive(false);

    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        menu.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(startScene); 
    }
   
}
