using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;

public class UIButtonShopItem : MonoBehaviour
{
    [Header("UI Riferimenti")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;
    public Image iconImage;

    private CollectableData currentCollectable;
    private PermanentBoostObject currentBoost;

    public CollectableData GetData() => currentCollectable;
    public PermanentBoostObject GetBoost() => currentBoost;

    public void SetData(CollectableData collectable)
    {
        currentCollectable = collectable;
        currentBoost = null;

        if (nameText != null)
            nameText.text = collectable.namePowerUp;

        if (iconImage != null)
            iconImage.sprite = collectable.icon;

        if (costText != null)
            costText.text = collectable.shopped ? "Comprato" : collectable.cost + " $";

        if (levelText == null) return;
    }


    public void SetDataBoost(PermanentBoostObject boost)
    {
        currentBoost = boost;
        currentCollectable = null;

        if (nameText != null)
            nameText.text = boost.nome;

        if (iconImage != null)
            iconImage.sprite = boost.icon;
        
        if (boost.currentLevel >= boost.maxLevel)
        {
            costText.text = ($"{boost.nome} è già al livello massimo!");
        }
        else 
        {
          costText.text = boost.cost + " $";
        }
            
        if (levelText != null)
            levelText.text = "Livello: " + boost.currentLevel;
    }
}

