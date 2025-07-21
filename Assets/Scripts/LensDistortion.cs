// 스크립트로 왜곡 적용
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ScreenDistorter : MonoBehaviour
{
    public PostProcessVolume volume;
    private LensDistortion distortion;

    void Start()
    {
        volume.profile.TryGetSettings(out distortion);
    }

    void Update()
    {
        distortion.intensity.value = Mathf.Sin(Time.time * 2f) * 50f; // 일그러짐 애니메이션
    }
}
