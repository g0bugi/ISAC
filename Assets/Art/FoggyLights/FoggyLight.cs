using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FoggyLight : MonoBehaviour
{
    public enum BlendModeEnum { Additive, AlphaBlended };
    public BlendModeEnum BlendMode = 0;
    public bool ApplyTonemap = true;

    public Color PointLightColor = Color.white;
    [Range(0, 8)]
    public float PointLightIntensity = 1;
    [Range(0, 20)]
    public float FoggyLightIntensity = 1;
    public float PointLightExponent = 5, Offset = -2;
    [Range(1, 40)]
    public float IntersectionRange = 2;
    public int DrawOrder = 1;
    public bool AttatchLight = false;

    private Light AttachedLight = null;
    private Material FoggyLightMaterial;
    private Renderer _renderer;

    // --- MaterialPropertyBlock 추가 ---
    private MaterialPropertyBlock propertyBlock;

    void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock(); // 프로퍼티 블록 초기화
        CreateMaterial();
    }

    void CreateMaterial()
    {
        if (!FoggyLightMaterial)
        {
            // 중요: 이제 모든 인스턴스가 이 하나의 머티리얼을 공유하게 됩니다.
            // 하지만 프로퍼티 블록 덕분에 각자 다른 모습으로 보일 수 있습니다.
            FoggyLightMaterial = new Material(Shader.Find("Hidden/FoggyLight"));
            FoggyLightMaterial.name = "Shared FoggyLight Material";
            // HideFlags를 설정하지 않아 씬에 남아있도록 할 수 있습니다.
        }
        _renderer.sharedMaterial = FoggyLightMaterial;
    }

    void Update() // OnWillRenderObject 대신 Update 사용
    { 
        if (_renderer == null) return;

        // 프로퍼티 블록에 현재 오브젝트의 고유한 값을 설정합니다.
        // 머티리얼에 직접 설정하는 대신, 블록에 설정하는 것이 핵심입니다.
        _renderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetColor("_PointLightColor", PointLightColor);
        propertyBlock.SetVector("_PointLightPosition", transform.position);
        propertyBlock.SetFloat("_PointLightIntensity", PointLightIntensity * FoggyLightIntensity);
        propertyBlock.SetFloat("_PointLightExponent", Mathf.Max(1, PointLightExponent));
        propertyBlock.SetFloat("_Offset", Offset);
        propertyBlock.SetFloat("_IntersectionRange", IntersectionRange);

        // 키워드 설정은 머티리얼에 직접 해야 할 수 있습니다.
        // 하지만 이 부분은 모든 인스턴스가 공유해도 괜찮을 수 있습니다.
        // 만약 각 인스턴스마다 블렌드 모드를 다르게 하고 싶다면, 이 로직도 변경이 필요합니다.
        BlendValues(FoggyLightMaterial, BlendMode);
        if (ApplyTonemap)
            FoggyLightMaterial.EnableKeyword("TONEMAP");
        else
            FoggyLightMaterial.DisableKeyword("TONEMAP");

        // 최종적으로 렌더러에 이 오브젝트만을 위한 프로퍼티 블록을 적용합니다.
        _renderer.SetPropertyBlock(propertyBlock);
        _renderer.sortingOrder = DrawOrder;

        // 연결된 라이트 관리 로직 (기존과 동일)
        HandleAttachedLight();
    }

    void HandleAttachedLight()
    {
        if (AttatchLight)
        {
            if (!gameObject.GetComponent<Light>())
            {
                gameObject.AddComponent<Light>();
                gameObject.GetComponent<Light>().shadows = LightShadows.Hard;
            }
            AttachedLight = gameObject.GetComponent<Light>();
            AttachedLight.intensity = PointLightIntensity / 2;
            AttachedLight.color = PointLightColor;
            AttachedLight.enabled = true;
        }
        else
        {
            if (AttachedLight)
                AttachedLight.enabled = false;
        }
    }

    // 머티리얼을 인자로 받도록 수정
    void BlendValues(Material mat, BlendModeEnum blendMode)
    {
        switch (blendMode)
        {
            case BlendModeEnum.Additive:
                mat.EnableKeyword("_ADDITIVE");
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                break;
            case BlendModeEnum.AlphaBlended:
                mat.DisableKeyword("_ADDITIVE");
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;
        }
    }
}