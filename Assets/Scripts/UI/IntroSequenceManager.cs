using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroSequenceManager : MonoBehaviour
{
    public FadeManager fadeManager; // FadeManager 참조
    public Button startButton;     // 게임 시작 버튼
    public GameObject startScreenPanel; // 게임 시작 버튼이 있는 초기 화면 패널

    [Tooltip("화면을 덮을 이미지 패널 (FadeManager.fadePanel과 동일해도 됨)")]
    public Image whiteOverlayPanel; // 하얀색 배경 역할을 할 Image 컴포넌트

    [System.Serializable]
    public class IntroUIStep
    {
        [Tooltip("이 단계에서 활성화될 UI 요소 그룹 (패널 등)")]
        public GameObject uiGroupToShow; // 이 단계에서 보여줄 UI (텍스트, 이미지 등)
        public float fadeDuration = 1.0f; // 하얀 배경 페이드인/아웃 시간
        public float displayDuration = 2.0f; // UI가 보여지는 시간 (하얀 배경 사라진 후)
    }

    public IntroUIStep[] introSteps; // 인트로 연출 단계 배열
    public string nextSceneName;     // 모든 연출 완료 후 로드할 다음 씬 이름
    private CanvasGroup startScreenCanvasGroup;

    private void Awake()
    {
        // 모든 인트로 UI 그룹은 시작 시 비활성화
        foreach (var step in introSteps)
        {
            if (step.uiGroupToShow != null)
            {
                step.uiGroupToShow.SetActive(false);
            }
        }

        // 하얀 오버레이는 시작 시 비활성화 (버튼 누르면 활성화됨)
        if (whiteOverlayPanel != null)
        {
            whiteOverlayPanel.gameObject.SetActive(false);
            whiteOverlayPanel.color = new Color(whiteOverlayPanel.color.r, whiteOverlayPanel.color.g, whiteOverlayPanel.color.b, 1f); // 시작 시 완전 불투명 상태
        }

        if (startScreenPanel != null)
        {
            startScreenCanvasGroup = startScreenPanel.GetComponent<CanvasGroup>();
            if (startScreenCanvasGroup == null)
            {
                startScreenCanvasGroup = startScreenPanel.AddComponent<CanvasGroup>();
            }
        }

        // 시작 버튼에 리스너 추가
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogError("Start Button이 할당되지 않았습니다!");
        }
    }

    private void OnStartButtonClicked()
    {
        if (startButton != null)
        {
            startButton.interactable = false; // 버튼 중복 클릭 방지
        }
        
        // --- 여기서 코루틴을 시작합니다. yield return을 직접 사용하지 않습니다. ---
        StartCoroutine(FadeOutStartScreenAndRunIntro());
    }
    
    private IEnumerator FadeOutStartScreenAndRunIntro()
    {
        if (startScreenPanel != null && startScreenCanvasGroup != null)
        {
            float timer = 0f;
            float duration = 0.5f; // 시작 화면이 사라지는 시간 (원하는 대로 조절)
            startScreenCanvasGroup.alpha = 1f; // 시작 시 완전 불투명하게 설정

            while (timer < duration)
            {
                timer += Time.deltaTime;
                startScreenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / duration);
                yield return null;
            }
            startScreenCanvasGroup.alpha = 0f; // 완전히 투명하게 설정
            startScreenPanel.SetActive(false); // 완전히 사라진 후 비활성화
            Debug.Log("시작 화면 페이드아웃 완료.");
        }
        else if (startScreenPanel == null)
        {
             Debug.LogWarning("Start Screen Panel이 할당되지 않아 페이드아웃 없이 진행됩니다.");
        }

        // --- 여기부터 UI 시퀀스 시작 전의 로직이 이어집니다. ---
        if (fadeManager == null || whiteOverlayPanel == null)
        {
            Debug.LogError("FadeManager 또는 White Overlay Panel이 할당되지 않았습니다. 시퀀스를 시작할 수 없습니다.");
            SceneManager.LoadScene(nextSceneName);
            yield break;
        }

        // 하얀 배경을 바로 덮도록 알파를 0으로 설정하고 FadeOut (알파 0->1)으로 진행
        whiteOverlayPanel.color = new Color(whiteOverlayPanel.color.r, whiteOverlayPanel.color.g, whiteOverlayPanel.color.b, 0f);
        yield return StartCoroutine(fadeManager.FadeOut(whiteOverlayPanel, 1.0f)); // 1초 동안 하얀 오버레이가 완전히 덮음
        
        // --- UI 시퀀스 시작 ---
        StartCoroutine(RunIntroSequence());
    }

    private IEnumerator RunIntroSequence()
    {
        if (fadeManager == null || whiteOverlayPanel == null)
        {
            Debug.LogError("FadeManager 또는 White Overlay Panel이 할당되지 않았습니다. 시퀀스를 시작할 수 없습니다.");
            SceneManager.LoadScene(nextSceneName); // 안전 장치: 바로 씬 전환
            yield break;
        }

        foreach (IntroUIStep step in introSteps)
        {
            if (step.uiGroupToShow == null)
            {
                Debug.LogWarning("UI Group To Show가 할당되지 않은 인트로 단계가 있습니다. 건너뜝니다.");
                continue;
            }

            // 1. 하얀 배경 페이드인 (화면이 하얗게 덮임)
            whiteOverlayPanel.color = new Color(whiteOverlayPanel.color.r, whiteOverlayPanel.color.g, whiteOverlayPanel.color.b, 0f); // 완전히 투명한 상태에서 시작
            yield return StartCoroutine(fadeManager.FadeOut(whiteOverlayPanel, step.fadeDuration)); // FadeOut 함수가 Image를 받도록 수정 필요

            // 2. 이전 UI 비활성화 및 새 UI 활성화 (하얀 배경 뒤에서)
            // 직전 step의 UI를 비활성화하는 로직이 필요하다면 여기에 추가 (uiGroupToShow가 바뀌는 경우)
            // (첫 번째 단계에서는 이전 UI가 없으므로 필요 없음)

            // 모든 UI 그룹을 일단 비활성화하여 겹치지 않게 합니다.
            foreach (var s in introSteps)
            {
                if (s.uiGroupToShow != null && s.uiGroupToShow != step.uiGroupToShow)
                {
                    s.uiGroupToShow.SetActive(false);
                }
            }
            step.uiGroupToShow.SetActive(true); // 현재 단계의 UI 활성화

            // 3. 하얀 배경 페이드아웃 (하얀 배경이 사라지면서 UI가 드러남)
            whiteOverlayPanel.color = new Color(whiteOverlayPanel.color.r, whiteOverlayPanel.color.g, whiteOverlayPanel.color.b, 1f); // 완전히 불투명한 상태에서 시작
            yield return StartCoroutine(fadeManager.FadeIn(whiteOverlayPanel, step.fadeDuration)); // FadeIn 함수가 Image를 받도록 수정 필요

            // 4. UI가 보여지는 시간 동안 대기
            yield return new WaitForSeconds(step.displayDuration);
        }

        // --- 모든 시퀀스 완료 후 씬 전환 ---
        Debug.Log("모든 인트로 시퀀스 완료. 다음 씬 로드: " + nextSceneName);

        // 마지막으로 하얀 배경 페이드인 (화면을 다시 하얗게 덮음)
        whiteOverlayPanel.color = new Color(whiteOverlayPanel.color.r, whiteOverlayPanel.color.g, whiteOverlayPanel.color.b, 0f); // 완전히 투명한 상태에서 시작
        yield return StartCoroutine(fadeManager.FadeOut(whiteOverlayPanel, 1.0f)); // 1초 동안 하얗게

        SceneManager.LoadScene(nextSceneName);
    }
}