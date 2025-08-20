using System.Collections; // 코루틴을 사용하기 위해 꼭 필요합니다!
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public Animator backgroundAnimator;
    public Image backgroundImage;

    // Inspector 창에서 페이드 속도를 조절할 수 있습니다. (예: 1 = 1초)
    public float fadeDuration = 1.0f;

    // 버튼을 누르면 이 함수가 호출됩니다.
    public void OnStartButtonClick()
    {
        // 직접 색을 바꾸는 대신, FadeToWhite 코루틴을 "시작"시키기만 합니다.
        StartCoroutine(FadeToWhite());
    }

    // 서서히 하얀색으로 바꾸는 코루틴 함수
    IEnumerator FadeToWhite()
    {
        // 애니메이션을 먼저 비활성화해서 더 이상 스프라이트가 바뀌지 않게 합니다.
        backgroundAnimator.enabled = false;

        float timer = 0f;
        Color startColor = backgroundImage.color; // 현재 색상 저장

        // 타이머가 설정한 시간(fadeDuration)에 도달할 때까지 반복합니다.
        while (timer < fadeDuration)
        {
            // 매 프레임마다 타이머를 조금씩 증가시킵니다.
            timer += Time.deltaTime;

            // Lerp 함수를 사용해 시작 색상에서 하얀색으로 중간 색상을 계산합니다.
            // timer / fadeDuration은 0.0 ~ 1.0 사이의 진행률을 나타냅니다.
            backgroundImage.color = Color.Lerp(startColor, Color.white, timer / fadeDuration);

            // 다음 프레임까지 잠시 멈추고 기다립니다.
            yield return null;
        }

        // 루프가 끝난 후, 완벽한 단색 배경을 위해 최종 처리를 합니다.
        backgroundImage.sprite = null;
        backgroundImage.color = Color.white;
    }
}