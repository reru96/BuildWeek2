using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIButtonShopItem : MonoBehaviour
{
    [Header("Riferimenti UI")]
    public TextMeshProUGUI buttonText;
    public Image iconImage;

    private CollectableData data; 

    public void SetData(CollectableData collectable)
    {
        data = collectable;

        if (collectable.shopped)
            buttonText.text = $"{collectable.namePowerUp} (Acquistato)";
        else
            buttonText.text = $"{collectable.namePowerUp} - {collectable.cost} coins";

        if (collectable.icon != null)
            iconImage.sprite = collectable.icon;
    }

    public CollectableData GetData() => data;
}

