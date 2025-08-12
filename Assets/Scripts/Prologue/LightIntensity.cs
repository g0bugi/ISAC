using System.Collections;
using UnityEngine;

public class LightIntensity : MonoBehaviour
{
    public Light dirlight;

    IEnumerator LightOn()
    {
        float time = 3f;
        float active = 0f;
        while (active < time)
        {
            dirlight.intensity = Mathf.Lerp(0f, 3f, active / time);
            active += Time.deltaTime;
        }
        yield return null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LightOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
