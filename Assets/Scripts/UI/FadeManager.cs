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

    public IEnumerator FadeOut(Image targetImage, float duration)
    {
        if (!targetImage.gameObject.activeSelf)
        {
            targetImage.gameObject.SetActive(true);
            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, 0f);
        }
        float timer = 0f;
        Color color = targetImage.color;
        float startAlpha = color.a;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 1, timer / duration);
            targetImage.color = color;
            yield return null;
        }
        color.a = 1;
        targetImage.color = color;
    }

    // --- Image 컴포넌트를 인수로 받는 FadeIn 함수 추가 ---
    public IEnumerator FadeIn(Image targetImage, float duration)
    {
        if (!targetImage.gameObject.activeSelf)
        {
            targetImage.gameObject.SetActive(true);
            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, 1f);
        }
        float timer = 0f;
        Color color = targetImage.color;
        float startAlpha = color.a;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 0, timer / duration);
            targetImage.color = color;
            yield return null;
        }
        color.a = 0;
        targetImage.color = color;
        targetImage.gameObject.SetActive(false);
    }
}