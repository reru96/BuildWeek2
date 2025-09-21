using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILives : MonoBehaviour
{
    [SerializeField] private GameObject shieldSpriteIcon;
    [SerializeField] private TextMeshProUGUI livesNumber;
    [SerializeField] private Transform shieldParent;

    private LifeController lifeController;
    private List<GameObject> icons = new List<GameObject>();

    private void Awake()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            lifeController = player.GetComponent<LifeController>();

    }

    void Start()
    {
        UpdateLives();
    }

    private void Update()
    {
        UpdateLives();
    }

    public void UpdateLives()
    {

        int hp = lifeController.GetHp();
        int shieldHp = lifeController.GetShield();
        livesNumber.text = hp.ToString();

        foreach (var icon in icons)
            Destroy(icon);

        icons.Clear();

        for (int i = 0; i < shieldHp; i++)
        {
            GameObject newIcon = Instantiate(shieldSpriteIcon, shieldParent);
            newIcon.SetActive(true);
            icons.Add(newIcon);
        }

    }
}
