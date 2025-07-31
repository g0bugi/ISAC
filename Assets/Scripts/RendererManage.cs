using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RendererManage : MonoBehaviour
{
    public FullScreenPassRendererFeature feature;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        feature.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
