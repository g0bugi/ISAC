using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInAndDestroy : MonoBehaviour
{
    private Image targetImage;

    public float fadeInDuration = 2.0f;

    public float initialDelay = 1.0f;
    public Playermove playerMovementAndRotation;

    void Start()
    {
        targetImage = GetComponent<Image>();

        if (targetImage == null)
        {
            Debug.LogError("이미지가 없음");
            return;
        }


        if (playerMovementAndRotation != null)
        {
            playerMovementAndRotation.enabled = false;
        }


        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {

        yield return new WaitForSeconds(initialDelay);

        float timer = 0f;
        Color initialColor = targetImage.color;


        while (timer < fadeInDuration)
        {
            float progress = timer / fadeInDuration;

            float currentAlpha = Mathf.Lerp(1f, 0f, progress);

            targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, currentAlpha);

            timer += Time.deltaTime;
            yield return null;
        }

        targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        if (playerMovementAndRotation != null)
        {
            playerMovementAndRotation.StartGettingUpAnimation();
        }

        Debug.Log($"playerMovementAndRotation: {playerMovementAndRotation}");
        Destroy(gameObject);
        
    }
}