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
        SaveManager.Save(new SaveData());
        SceneManager.LoadScene(1); // scena iniziale
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
