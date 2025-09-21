using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAudioSettings : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    private SaveData saveData;

    private void Start()
    {
        saveData = SaveManager.Load();

       
        musicSlider.value = saveData.musicVolume;
        sfxSlider.value = saveData.sfxVolume;

        ApplyVolume("Music", saveData.musicVolume);
        ApplyVolume("Sfx", saveData.sfxVolume);

        musicSlider.onValueChanged.AddListener(v => ApplyVolume("Music", v));
        sfxSlider.onValueChanged.AddListener(v => ApplyVolume("Sfx", v));

    }

    private void ApplyVolume(string type, float value)
    {
        if (type == "Music")
        {
            AudioManager.Instance.SetMusicVolume(value);
            saveData.musicVolume = value;
        }
        else if (type == "Sfx")
        {
            AudioManager.Instance.SetSfxVolume(value);
            saveData.sfxVolume = value;
        }

        SaveManager.Save(saveData);
    }
}
