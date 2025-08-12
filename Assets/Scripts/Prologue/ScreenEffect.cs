using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenEffect : MonoBehaviour
{
    public Volume volume;
    private UnityEngine.Rendering.Universal.LensDistortion distortion;
    private FilmGrain grain;
    public float waittime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volume.profile.TryGet(out distortion);
        volume.profile.TryGet(out grain);
        distortion.intensity.value = 0f;
        grain.intensity.value = 0f;
        StartCoroutine(Distort());
    }
    IEnumerator Distort()
    {
        yield return new WaitForSeconds(waittime);

        grain.intensity.value = 1f;
        //Debug.Log(grain.intensity.value);

        float duration = 10f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            distortion.intensity.value = Mathf.Sin(elapsed);
            distortion.xMultiplier.value = Mathf.Cos(elapsed);
            //distortion.yMultiplier.value = Mathf.Sin(elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield break;
    }
   
}
