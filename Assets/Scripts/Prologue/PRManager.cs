using System.Collections;
using UnityEngine;

public class PRManager : MonoBehaviour
{
    public FullScreenPassRendererFeature render;
    public float time = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        render.SetActive(false);
        StartCoroutine(RenderOn());
    }

    IEnumerator RenderOn()
    {
        yield return new WaitForSeconds(time);

        render.SetActive(true);

        yield break;
    }
}
