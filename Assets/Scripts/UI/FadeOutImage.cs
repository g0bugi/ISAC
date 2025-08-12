using UnityEngine;
using UnityEngine.UI; // Image 컴포넌트를 사용하기 위해 필요
using System.Collections; // 코루틴을 사용하기 위해 필요

public class FadeOutImage : MonoBehaviour
{
    // 페이드아웃에 걸리는 시간
    public float fadeDuration = 2.0f;
    
    private Image targetImage;

    void Start()
    {
        // 스크립트가 붙어있는 GameObject에서 Image 컴포넌트를 가져옴
        targetImage = GetComponent<Image>();

        if (targetImage == null)
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없습니다. 이 스크립트는 Image 컴포넌트에 붙여야 합니다.");
            return;
        }
        

        // 씬 시작과 동시에 페이드아웃 코루틴 시작
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2.0f);

        float timer = 0f;
        Color startColor = targetImage.color;
        
        // 초기 투명도를 1(완전 불투명)로 설정
        startColor.a = 1f;
        targetImage.color = startColor;

        while (timer < fadeDuration)
        {
            // 시간 경과에 따라 알파 값을 1에서 0으로 선형 보간
            timer += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            
            // 이미지의 색상에 새로운 알파 값을 적용
            targetImage.color = new Color(startColor.r, startColor.g, startColor.b, currentAlpha);
            
            yield return null; // 다음 프레임까지 대기
        }

        // 코루틴이 끝난 후 알파 값을 0으로 고정
        targetImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        // 페이드아웃이 완료되면 GameObject를 비활성화 (선택 사항)
        gameObject.SetActive(false);
    }
}