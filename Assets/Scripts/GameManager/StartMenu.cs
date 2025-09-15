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

        // carica il salvataggio se esiste
        lastSave = SaveManager.Load();

        // Se non esiste un salvataggio valido, lastSave sarà "vuoto"
        if (lastSave == null || lastSave.sceneIndex == 0)
        {
            lastSave = null; // nessun salvataggio valido
        }
    }

    public void NewGame()
    {
        // Ricomincia da zero → crea un nuovo SaveData vuoto
        SaveData newSave = new SaveData();
        newSave.coins = 0;        // azzera coins
        newSave.score = 0;        // azzera punteggio
        newSave.sceneIndex = 1;   // prima scena di gioco (modifica se serve)

        SaveManager.Save(newSave);

        // Se il CoinManager esiste già in scena, resetta anche lui
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
            // Carica la scena salvata
            SceneManager.LoadScene(lastSave.sceneIndex);

            // 👇 opzionale: il GameController legge questo dopo il caricamento
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
