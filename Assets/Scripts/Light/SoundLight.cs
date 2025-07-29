using UnityEngine;
using UnityEngine.UI; // UI 요소를 사용하기 위해 필요합니다.
using System.Collections; // 코루틴을 사용하기 위해 필요합니다.

public class SoundLight : MonoBehaviour
{
    public GameObject ripplePrefab; // 생성할 파장 UI Prefab
    public float rippleDuration = 1.0f; // 파장 효과 지속 시간 (초)
    public float maxRippleSize = 300f; // 파장이 최대로 커지는 크기 (UI 픽셀)
    public float minRippleSize = 50f;
    public float rippleScaleMultiplier = 2.0f;
    public float randomOffsetRange = 50f; // 파장이 나타날 랜덤 범위 (픽셀 단위)
    public float rippleTransparency = 1.0f; // 투명도

    public float screenEdgePadding = 50f;
    private RectTransform canvasRectTransform;
    
    

    private Camera mainCamera; // 메인 카메라 참조

    void Start()
    {
        // 씬에 있는 메인 카메라를 찾습니다.
        // 태그가 "MainCamera"로 설정되어 있어야 합니다.
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("씬에서 'MainCamera' 태그가 지정된 카메라를 찾을 수 없습니다.");
        }

        canvasRectTransform = transform.parent.GetComponent<RectTransform>();
        if (canvasRectTransform == null)
        {
            Debug.LogError("Canvas의 RectTransform을 찾을 수 없습니다. SoundRippleEffectManager가 Canvas의 자식에 붙어있는지 확인하세요.");
        }
    
    }

    
    // 이 함수는 외부에서 호출되어 파장 효과를 시작합니다.
    // soundSourcePosition: 소리가 나는 3D 월드 위치
    public void PlayRippleEffect(Vector3 soundSourcePosition, float audioSourceMaxDistance)
    {
        if (ripplePrefab == null || mainCamera == null || canvasRectTransform == null)
        {
            Debug.LogError("필수 컴포넌트 또는 프리팹이 할당되지 않았습니다.");
            return;
        }

        float distanceToCamera = Vector3.Distance(soundSourcePosition, mainCamera.transform.position);

        if (distanceToCamera > audioSourceMaxDistance)
        {
            return; // 소리 감쇠 거리를 넘어가면 파장 생성 안 함
        }

        float normalizedDistance = Mathf.InverseLerp(audioSourceMaxDistance, 0f, distanceToCamera);
        float initialRippleSize = Mathf.Lerp(minRippleSize, maxRippleSize, normalizedDistance); 

        GameObject rippleGO = Instantiate(ripplePrefab, transform.parent); 
        RectTransform rippleRect = rippleGO.GetComponent<RectTransform>();
        Image rippleImage = rippleGO.GetComponent<Image>();

        rippleRect.sizeDelta = new Vector2(initialRippleSize, initialRippleSize);
        rippleTransparency = 1.0f;

        // --- 핵심 로직 수정: 카메라 뒤 오브젝트 위치 반전 처리 ---
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(soundSourcePosition); // 3D 월드 -> 2D 화면 픽셀 좌표

        // 오브젝트가 카메라 뒤에 있는지 확인
        // WorldToScreenPoint의 Z값이 0보다 작으면 카메라 뒤에 있는 것입니다.
        if (screenPoint.z < 0)
        {
            // Debug.Log("카메라 뒤 오브젝트 감지: " + soundSourcePosition + " -> " + screenPoint);
            // 카메라 뒤에 있는 경우, 화면 중앙을 기준으로 X, Y 좌표를 반전시킵니다.
            // 이렇게 하면 소스 위치가 화면 뒤편의 왼쪽 위라면, 화면 앞의 오른쪽 아래로 매핑됩니다.
            screenPoint.x = Screen.width - screenPoint.x;
            screenPoint.y = Screen.height - screenPoint.y;
            rippleTransparency = 0.5f;

        }
        

        // 최종 화면 위치를 파장 RectTransform에 적용
        rippleRect.position = screenPoint; 

        // 랜덤 오프셋 추가는 최종 위치 결정 후에 적용
        float offsetX = Random.Range(-randomOffsetRange, randomOffsetRange);
        float offsetY = Random.Range(-randomOffsetRange, randomOffsetRange);
        rippleRect.position += new Vector3(offsetX, offsetY, 0);

        StartCoroutine(AnimateRipple(rippleRect, rippleImage, initialRippleSize));
    }

    IEnumerator AnimateRipple(RectTransform rect, Image image, float startSizeForAnim)
    {
        //Debug.Log("AnimateRipple 코루틴 시작됨!");
        float timer = 0f;
        float endSizeForAnim = startSizeForAnim * rippleScaleMultiplier;
        Color initialColor = image.color; // 초기 색상 (알파 0으로 설정된 색상)

        // 파장 시작 시 알파 값을 1로 설정하여 보이게 합니다.
        image.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);

        while (timer < rippleDuration)
        {
            // 경과 시간 비율
            float progress = timer / rippleDuration;

            // 크기 애니메이션 (선형 보간)
            float currentSize = Mathf.Lerp(startSizeForAnim, endSizeForAnim, progress); 
            rect.sizeDelta = new Vector2(currentSize, currentSize);

            // 투명도 애니메이션 (1에서 0으로 감소)
            // 파장이 나타났다가 사라지는 효과를 위해 1-progress를 사용합니다.
            float currentAlpha = Mathf.Lerp(rippleTransparency, 0f, progress); 
            image.color = new Color(initialColor.r, initialColor.g, initialColor.b, currentAlpha);

            timer += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 애니메이션 완료 후 오브젝트 파괴
        Destroy(rect.gameObject);
    }
}