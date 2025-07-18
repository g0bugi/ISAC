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

    public float rippleInterval = 0.5f; // 파장 효과가 나타나는 간격 (초)
    
    

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
    
    }

    
    // 이 함수는 외부에서 호출되어 파장 효과를 시작합니다.
    // soundSourcePosition: 소리가 나는 3D 월드 위치
    public void PlayRippleEffect(Vector3 soundSourcePosition, float audioSourceMaxDistance)
    {
        if (ripplePrefab == null)
        {
            Debug.LogError("Ripple Prefab이 할당되지 않았습니다. Inspector에서 할당해주세요.");
            return;
        }
        if (mainCamera == null)
        {
            Debug.LogError("메인 카메라가 없습니다. 파장 효과를 생성할 수 없습니다.");
            return;
        }
        // 1. 소리 오브젝트와 카메라(AudioListener) 사이의 실제 거리 계산
        float distanceToCamera = Vector3.Distance(soundSourcePosition, mainCamera.transform.position);

        // 2. Max Distance를 넘어가면 파장 생성하지 않음
        if (distanceToCamera > audioSourceMaxDistance)
        {
            // Debug.Log("거리가 Max Distance(" + audioSourceMaxDistance + ")를 초과하여 파장 생성 안 함: " + distanceToCamera);
            return; // 함수 종료, 파장 생성하지 않음
        }

        // 3. 거리 기반으로 파장 크기 계산
        // 거리가 0에 가까울수록 maxRippleSize, MaxDistance에 가까울수록 minRippleSize
        // Mathf.InverseLerp는 값이 min ~ max 범위 내에서 어디에 위치하는지 0~1 사이의 값으로 반환합니다.
        // 여기서는 거리가 멀수록 0에 가까워지도록, 가까울수록 1에 가까워지도록 역전시킵니다.
        float normalizedDistance = Mathf.InverseLerp(audioSourceMaxDistance, 0f, distanceToCamera);
        
        float initialRippleSize = Mathf.Lerp(minRippleSize, maxRippleSize, normalizedDistance);

        // Debug.Log("거리: " + distanceToCamera + ", 정규화된 거리: " + normalizedDistance + ", 최종 파장 크기: " + currentRippleSize);



        // 3D 월드 위치를 2D 화면상의 픽셀 위치로 변환합니다.
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(soundSourcePosition);

        // 랜덤 부분
        float offsetX = Random.Range(-randomOffsetRange, randomOffsetRange);
        float offsetY = Random.Range(-randomOffsetRange, randomOffsetRange);
        screenPosition += new Vector2(offsetX, offsetY);

        
        // 파장 UI 인스턴스를 생성합니다.
        // Canvas의 자식으로 생성되어야 화면에 제대로 표시됩니다.
        GameObject rippleGO = Instantiate(ripplePrefab, transform.parent); // 이 스크립트가 붙은 오브젝트의 부모 (Canvas) 아래에 생성
        RectTransform rippleRect = rippleGO.GetComponent<RectTransform>();
        Image rippleImage = rippleGO.GetComponent<Image>();

        

        // 빛 UI의 초기 위치 및 사이즈 설정
        rippleRect.position = screenPosition;
        rippleRect.sizeDelta = new Vector2(initialRippleSize, initialRippleSize);


        // 파장 애니메이션 코루틴 시작
        StartCoroutine(AnimateRipple(rippleRect, rippleImage, initialRippleSize));

        Debug.Log("변환된 스크린 위치: " + screenPosition);
    }

    IEnumerator AnimateRipple(RectTransform rect, Image image, float startSizeForAnim)
    {
        Debug.Log("AnimateRipple 코루틴 시작됨!");
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
            float currentAlpha = Mathf.Lerp(1f, 0f, progress); 
            image.color = new Color(initialColor.r, initialColor.g, initialColor.b, currentAlpha);

            timer += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 애니메이션 완료 후 오브젝트 파괴
        Destroy(rect.gameObject);
    }
}