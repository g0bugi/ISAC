using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutAndDestroy : MonoBehaviour
{
    // 이미지를 서서히 사라지게 할 Image 컴포넌트
    private Image targetImage;

    // 이미지가 완전히 사라지는 데 걸리는 시간
    public float fadeOutDuration = 2.0f;

    // 사라지기 시작하기 전 대기 시간
    public float initialDelay = 1.0f;
    public Playermove playerMovementAndRotation;

    void Start()
    {
        // 이 스크립트가 붙어있는 오브젝트의 Image 컴포넌트를 가져옵니다.
        targetImage = GetComponent<Image>();

        // Image 컴포넌트가 있는지 확인
        if (targetImage == null)
        {
            Debug.LogError("오브젝트에 Image 컴포넌트가 없습니다. 스크립트를 제거하거나 Image 컴포넌트를 추가해주세요.");
            return;
        }

        // 게임 시작 시 바로 플레이어의 움직임을 막습니다.
        if (playerMovementAndRotation != null)
        {
            playerMovementAndRotation.enabled = false;
        }

        // 코루틴을 시작하여 페이드아웃 효과를 실행합니다.
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        // 1. 페이드아웃 시작 전 대기
        // 지정된 시간만큼 기다립니다.
        yield return new WaitForSeconds(initialDelay);

        float timer = 0f;
        Color initialColor = targetImage.color; // 이미지의 현재 색상 (알파값 포함)

        // 2. 페이드아웃 효과 실행
        // 타이머가 fadeOutDuration에 도달할 때까지 반복
        while (timer < fadeOutDuration)
        {
            // 경과 시간을 전체 시간에 대한 비율(0~1)로 변환
            float progress = timer / fadeOutDuration;

            // 현재 알파값을 1(불투명)에서 0(완전 투명)으로 Lerp(선형 보간)
            float currentAlpha = Mathf.Lerp(1f, 0f, progress);

            // 이미지의 새 알파값 적용
            targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, currentAlpha);

            // 다음 프레임까지 대기
            timer += Time.deltaTime;
            yield return null;
        }

        // 3. 완전히 사라진 후 오브젝트 제거
        // 페이드아웃이 끝난 후 혹시라도 투명도가 완벽히 0이 안 되었을 경우를 대비해 0으로 고정
        targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        if (playerMovementAndRotation != null)
        {
            playerMovementAndRotation.StartGettingUpAnimation();
        }

        Debug.Log($"playerMovementAndRotation: {playerMovementAndRotation}");
        // 마지막으로 오브젝트를 파괴하여 메모리에서 제거
        Destroy(gameObject);
    }
}