// ��ũ��Ʈ�� �ְ� ����
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
        distortion.intensity.value = Mathf.Sin(Time.time * 2f) * 50f; // �ϱ׷��� �ִϸ��̼�
    }
}
