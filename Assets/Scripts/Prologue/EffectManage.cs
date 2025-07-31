using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EffectManage : MonoBehaviour
{
    public Material twirl;
    public float time = 40f;
    public float intensity = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        twirl.SetFloat("_Intensity", 0);
        StartCoroutine(IntenUp());
    }
    
    IEnumerator IntenUp()
    {
        yield return new WaitForSeconds(time);
        //Debug.Log("check");
        float fadetime = 5.0f;
        float activetime = 0.0f;
        while (activetime <= fadetime)
        {
            twirl.SetFloat("_Intensity", Mathf.Lerp(0f, 8f, activetime / fadetime));
            activetime += Time.deltaTime;
            //Debug.Log("check2");
            yield return null;
        }
    }
    // Update is called once per frame
   
}
