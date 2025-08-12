using UnityEngine;

public class RuntimeFoggyLightSpawner : MonoBehaviour
{
    // 이 변수에 Resources 폴더에 있는 FoggyLight.FBX 프리팹을 미리 할당해두세요.
    public GameObject foggyLightMeshPrefab;

    void Start()
    {
        // 원하는 위치에 FoggyLight를 생성합니다.
        CreateFoggyLightAt(new Vector3(0, 1, 5));
    }

    public GameObject CreateFoggyLightAt(Vector3 position)
    {
        // 1. 빈 게임 오브젝트를 생성합니다.
        GameObject newFoggyLight = new GameObject("Runtime Foggy Light");
        newFoggyLight.transform.position = position;

        // 2. 필수 컴포넌트들을 추가합니다.
        MeshFilter meshFilter = newFoggyLight.AddComponent<MeshFilter>();
        newFoggyLight.AddComponent<MeshRenderer>();
        FoggyLight foggyLightComponent = newFoggyLight.AddComponent<FoggyLight>();

        // 3. (가장 중요) Resources 폴더에서 FBX 모델을 로드하여 메시를 할당합니다.
        //    foggyLightMeshPrefab 변수를 사용하는 방법이 더 효율적입니다.
        if (foggyLightMeshPrefab != null)
        {
            meshFilter.sharedMesh = foggyLightMeshPrefab.GetComponent<MeshFilter>().sharedMesh;
        }
        else
        {
            // 또는 Resources.Load를 직접 사용할 수 있습니다.
            GameObject meshSource = Resources.Load<GameObject>("FoggyLight");
            if (meshSource != null)
            {
                meshFilter.sharedMesh = meshSource.GetComponent<MeshFilter>().sharedMesh;
            }
            else
            {
                Debug.LogError("Resources 폴더에서 'FoggyLight.FBX'를 찾을 수 없습니다!");
                Destroy(newFoggyLight);
                return null;
            }
        }

        // 4. FoggyLight.cs 스크립트의 공개 변수들을 필요에 따라 설정합니다.
        foggyLightComponent.PointLightColor = Color.yellow;
        foggyLightComponent.FoggyLightIntensity = 2.5f;
        // ... 기타 필요한 설정들 ...

        // 5. 렌더러 설정을 추가합니다. (FoggyLightCreator.cs 참고)
        Renderer renderer = newFoggyLight.GetComponent<Renderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        renderer.receiveShadows = false;

        Debug.Log("Foggy Light가 런타임에 생성되었습니다.");
        return newFoggyLight;
    }
}
