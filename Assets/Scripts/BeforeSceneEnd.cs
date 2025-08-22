using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BeforeSceneEnd : MonoBehaviour
{
    public GameObject BeforeSceneEndPanel;
    public Image endPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BeforeSceneEndPanel.SetActive(false);
        Color a = endPanel.color;
        a.a = 0;
        endPanel.color = a; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Sequence()
    {
        BeforeSceneEndPanel.SetActive(true);
        StartCoroutine(fadeout(1));
        yield return null;
        StartCoroutine(Wait(1f));
        yield return null;
        StartCoroutine(fadein(1));
        yield break;
    }

    IEnumerator fadeout(float wait)
    {
        float time = 0f;
        float active = wait;
        Color alpha;
        while(time < active)
        {
            alpha = endPanel.color;
            alpha.a = Mathf.Lerp(0f, 1f, time/active);
            endPanel.color = alpha;
            time += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator Wait(float wait)
    {
        yield return new WaitForSeconds(1f);

        yield break;
    }
    IEnumerator fadein(float wait)
    {
        float time = 0f;
        float active = wait;
        Color alpha;
        while (time < active)
        {
            alpha = endPanel.color;
            alpha.a = Mathf.Lerp(0f, 1f, time / active);
            endPanel.color = alpha;
            time += Time.deltaTime;
            yield return null;
        }
    }
}
