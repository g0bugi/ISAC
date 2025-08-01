using UnityEngine;

public class RenderManage : MonoBehaviour
{
    public FullScreenPassRendererFeature render;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        render.SetActive(false);
    }
}
