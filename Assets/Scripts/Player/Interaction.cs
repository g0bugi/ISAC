using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Interaction : MonoBehaviour
{
    [Header("Outline")]
    public Material outline;                  // 인스펙터에서 할당(필수)

    [Header("Raycast")]
    public Camera cam;                        // 인스펙터에서 메인 카메라 할당
    public LayerMask mask;                    // 상호작용 레이어
    public float maxdistance = 15f;

    private GameObject current;               // 현재 아웃라인 적용 중인 최상위 대상
    private Renderer[] currentRenderers;      // 캐시: 자식까지 포함한 렌더러들

    private RaycastHit hit;
    private Playermove move;

    public GameObject marker; //플레이어한테 조작키 안내할 위치
    public Sprite sprite; // 조작키 이미지

    private bool IsMarker = false;
    void Awake()
    {
        move = GetComponent<Playermove>();
        if (cam == null) cam = Camera.main;
        if (outline == null)
            Debug.LogWarning("[Interaction] outline 머티리얼이 비어 있습니다. 인스펙터에서 할당하세요.");
    }

    void Update()
    {
        // 1) 레이 쏘기
        bool hasHit = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxdistance, mask);
        if (hasHit && hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            GameObject target = hit.collider.gameObject;
           
            // 2) 대상이 바뀌었는지 확인
            if (current != target)
            {
                // 이전 대상에서 아웃라인 제거
                if (current != null)
                {
                    RemoveOutline(currentRenderers);
                    RemoveMarker();
                }
                // 새 대상 설정 & 아웃라인 추가
                current = target;
                currentRenderers = current.GetComponentsInChildren<Renderer>(true);
                AddOutline(currentRenderers);
                AddMarker(current);
            }

            // 3) 상호작용
            if (Input.GetKeyDown(KeyCode.F))
            {
                move.canMove = false;
                // 안전하게 인터페이스 호출
                if (current.TryGetComponent<IInteractiable>(out var interact))
                {
                    interact.Action();
                }
                else
                {
                    // 상위나 자식에 붙어 있을 수도 있으면 필요 시 확장:
                    var interactInParent = current.GetComponentInParent<IInteractiable>();
                    if (interactInParent != null) interactInParent.Action();
                }
                move.canMove = true;
            }
        }
        else
        {
            // 레이를 못 맞추거나 레이어가 다르면, 기존 대상에서 아웃라인 제거
            if (current != null)
            {
                RemoveOutline(currentRenderers);
                RemoveMarker();
                current = null;
                currentRenderers = null; 
            }
        }
    }

    // ---------- 머티리얼 조작 유틸 ----------
    void AddMarker(GameObject target)
    {
        if (!IsMarker)
        {
            marker = new GameObject("Marker");
            marker.AddComponent<SpriteRenderer>();
            SpriteRenderer sr = marker.GetComponent<SpriteRenderer>();
            var sa = sr.color;
            sa.a = 0f;
            sr.color = sa;
            Vector3 up = new Vector3(0, 2, 0);
            marker.transform.position = target.transform.GetChild(0).transform.position + up;
           
            
            sr.sprite = sprite;
            sr.transform.localScale *= 0.5f;
            StartCoroutine(Alpha(sr, true));
            IsMarker = true;
        }
        else
            return;
    }
    void RemoveMarker()
    {
        Destroy(marker);
        IsMarker = false;
    }   
    void AddOutline(Renderer[] renderers)
    {
        if (outline == null || renderers == null) return;

        foreach (var r in renderers)
        {
            // materials는 인스턴스를 만들어 돌려줌(여기서만 사용)
            var mats = new List<Material>(r.materials);
            if (!mats.Contains(outline))
            {
                mats.Add(outline);
                r.materials = mats.ToArray(); // 변경 시에만 재할당
            }
        }
    }

    void RemoveOutline(Renderer[] renderers)
    {
        if (outline == null || renderers == null) return;

        // outline 변수가 원본 머티리얼의 이름을 가지고 있다고 가정
        string outlineName = outline.name;

        foreach (var r in renderers)
        {
            var mats = new List<Material>(r.materials);
            bool removed = false;

            for (int i = mats.Count - 1; i >= 0; i--)
            {
                // 수정된 부분: 이름 비교 로직
                // mats[i].name이 원본 이름으로 시작하고 (Instance)를 포함하는지 확인
                // 혹은 단순히 포함하는지 확인하는 Contains() 사용
                if (mats[i].name.StartsWith(outlineName) && mats[i].name.Contains("(Instance)"))
                {
                    // 제거 전에 파괴하여 메모리 누수 방지
                    Destroy(mats[i]);
                    mats.RemoveAt(i);
                    removed = true;
                }
            }

            if (removed)
                r.materials = mats.ToArray();
        }
    }

    IEnumerator Alpha(SpriteRenderer a, bool up)
    {
        var sprite = a.color;
        float time = 0f;
        float runtime = 1f;
        while (time < runtime)
        {
            sprite.a = Mathf.Lerp(0f, 1f, time/runtime);
            a.color = sprite;
            time += Time.deltaTime;
            yield return null;
        }
        Debug.Log("알파값 바꿨다");
        yield break;
    }
}
