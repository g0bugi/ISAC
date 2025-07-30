using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fadein : MonoBehaviour
{
    public GameObject panel;
    public float runningtime = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel.GetComponent<CanvasRenderer>().SetAlpha(0f);
        panel.SetActive(true);

        StartCoroutine(PlaySequence()); // 코루틴 묶어서 순차 실행
    }
    IEnumerator PlaySequence()
    {
        yield return StartCoroutine(FadeIn());   // 2단계: 페이드 인
        yield return StartCoroutine(FadeOut());  // 3단계: 다시 페이드 아웃
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator FadeOut() 
    {
        float fadetime = 5.0f;
        float activetime = 0.0f;
        while (activetime <= fadetime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 5f, activetime / fadetime));
            activetime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        

    }
    IEnumerator FadeIn()
    {
        float fadetime = 5.0f;
        float activetime = 0.0f;
        while (activetime <= fadetime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(5f,0f,activetime/fadetime));
            activetime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(runningtime);
    }
}
