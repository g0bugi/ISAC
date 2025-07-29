using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadePanel;

    public IEnumerator FadeOut(float duration)
    {
        if (!fadePanel.gameObject.activeSelf)
        {
            fadePanel.gameObject.SetActive(true);
        }
        float timer = 0f;
        Color color = fadePanel.color;
        color.a = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / duration);
            fadePanel.color = color;
            yield return null;
        }
        color.a = 1;
        fadePanel.color = color;
    }

    public IEnumerator FadeIn(float duration)
    {
        if (!fadePanel.gameObject.activeSelf)
        {
            fadePanel.gameObject.SetActive(true);
        }
        float timer = 0f;
        Color color = fadePanel.color;
        color.a = 1;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / duration);
            fadePanel.color = color;
            yield return null;
        }
        color.a = 0;
        fadePanel.color = color;

        fadePanel.gameObject.SetActive(false);
    }
}