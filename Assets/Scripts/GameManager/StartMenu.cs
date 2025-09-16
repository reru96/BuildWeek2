using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject settingsMenu;

    private SaveData lastSave;

    private void Start()
    {
        settingsMenu.SetActive(false);

    
        lastSave = SaveManager.Load();

      
        if (lastSave == null || lastSave.sceneIndex == 0)
        {
            lastSave = null; 
        }
    }

    public void NewGame()
    {
  
        SaveData newSave = new SaveData();
        newSave.coins = 0;        
        newSave.score = 0;        
        newSave.sceneIndex = 1;   

        SaveManager.Save(newSave);

       
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.SetCoins(0);
        }

        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        if (lastSave != null)
        {
        
            SceneManager.LoadScene(lastSave.sceneIndex);

            GameController.pendingSaveData = lastSave;
        }
        else
        {
            Debug.Log("Nessun salvataggio trovato. Avvio nuova partita...");
            NewGame();
        }
    }

    public void ShowOptions()
    {
        settingsMenu.SetActive(true);
    }

    public void HideOptions()
    {
        settingsMenu.SetActive(false);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
