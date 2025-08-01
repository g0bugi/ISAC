using UnityEngine;

public class RuntimeFoggyLightSpawner : MonoBehaviour
{
    // �� ������ Resources ������ �ִ� FoggyLight.FBX �������� �̸� �Ҵ��صμ���.
    public GameObject foggyLightMeshPrefab;

    void Start()
    {
        // ���ϴ� ��ġ�� FoggyLight�� �����մϴ�.
        CreateFoggyLightAt(new Vector3(0, 1, 5));
    }

    public GameObject CreateFoggyLightAt(Vector3 position)
    {
        // 1. �� ���� ������Ʈ�� �����մϴ�.
        GameObject newFoggyLight = new GameObject("Runtime Foggy Light");
        newFoggyLight.transform.position = position;

        // 2. �ʼ� ������Ʈ���� �߰��մϴ�.
        MeshFilter meshFilter = newFoggyLight.AddComponent<MeshFilter>();
        newFoggyLight.AddComponent<MeshRenderer>();
        FoggyLight foggyLightComponent = newFoggyLight.AddComponent<FoggyLight>();

        // 3. (���� �߿�) Resources �������� FBX ���� �ε��Ͽ� �޽ø� �Ҵ��մϴ�.
        //    foggyLightMeshPrefab ������ ����ϴ� ����� �� ȿ�����Դϴ�.
        if (foggyLightMeshPrefab != null)
        {
            meshFilter.sharedMesh = foggyLightMeshPrefab.GetComponent<MeshFilter>().sharedMesh;
        }
        else
        {
            // �Ǵ� Resources.Load�� ���� ����� �� �ֽ��ϴ�.
            GameObject meshSource = Resources.Load<GameObject>("FoggyLight");
            if (meshSource != null)
            {
                meshFilter.sharedMesh = meshSource.GetComponent<MeshFilter>().sharedMesh;
            }
            else
            {
                Debug.LogError("Resources �������� 'FoggyLight.FBX'�� ã�� �� �����ϴ�!");
                Destroy(newFoggyLight);
                return null;
            }
        }

        // 4. FoggyLight.cs ��ũ��Ʈ�� ���� �������� �ʿ信 ���� �����մϴ�.
        foggyLightComponent.PointLightColor = Color.yellow;
        foggyLightComponent.FoggyLightIntensity = 2.5f;
        // ... ��Ÿ �ʿ��� ������ ...

        // 5. ������ ������ �߰��մϴ�. (FoggyLightCreator.cs ����)
        Renderer renderer = newFoggyLight.GetComponent<Renderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        renderer.receiveShadows = false;

        Debug.Log("Foggy Light�� ��Ÿ�ӿ� �����Ǿ����ϴ�.");
        return newFoggyLight;
    }
}
