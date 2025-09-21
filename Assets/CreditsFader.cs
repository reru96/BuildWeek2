using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsFader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private float fadeDuration = 1f;  
    [SerializeField] private float displayTime = 3f;   

    [TextArea(3, 10)]
    [SerializeField] private List<string> creditsLines = new List<string>();

    private void Start()
    {
        StartCoroutine(ShowCredits());
    }

    private IEnumerator ShowCredits()
    {
        foreach (string line in creditsLines)
        {
            yield return StartCoroutine(FadeIn(line));
            yield return new WaitForSeconds(displayTime);
            yield return StartCoroutine(FadeOut());
        }

        SceneManager.LoadScene("StartMenu");
    }

    private IEnumerator FadeIn(string newText)
    {
        creditsText.text = newText;
        creditsText.alpha = 0;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            creditsText.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        creditsText.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            creditsText.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        creditsText.alpha = 0;
    }
}
