using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenEffect : MonoBehaviour
{
    public Volume volume;
    private LensDistortion distortion;
    public float waittime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume.profile.TryGet(out distortion);
        distortion.intensity.value = 0f;
        StartCoroutine(Distort());
    }
    IEnumerator Distort()
    {
        yield return new WaitForSeconds(waittime);

        float duration = 10f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            distortion.intensity.value = Mathf.Sin(elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        distortion.intensity.value = 0f;
    }

}
