using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChangerOnTrigger : MonoBehaviour
{
    public string targetSceneName;
    public string targetTag = "Player"; // 여기에 특정 오브젝트의 태그를 입력합니다. (기본값: Player)
    public FadeManager fadeManager; // --- FadeManager 참조 추가 ---
    public float fadeOutDuration = 1.0f; // --- 페이드 아웃에 걸릴 시간 추가 ---

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 'targetTag'에 설정된 태그와 일치하는지 확인합니다.
        if (other.CompareTag(targetTag))
        {
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                // --- 씬 전환 전에 페이드 아웃 코루틴 시작 ---
                StartCoroutine(ChangeSceneWithFadeOut());
            }
            else
            {
                Debug.LogWarning("씬 이름이 설정되지 않았습니다. Inspector에서 'Target Scene Name'을 설정해주세요.");
            }
        }
        else
        {
            Debug.Log($"태그가 '{targetTag}'인 오브젝트만 씬을 변경할 수 있습니다. 현재 충돌한 오브젝트의 태그: {other.tag}");
        }
    }

    // --- 씬 전환과 페이드 아웃을 처리할 코루틴 ---
    private IEnumerator ChangeSceneWithFadeOut()
    {
        if (fadeManager != null)
        {
            // FadeOut 코루틴이 끝날 때까지 기다립니다.
            yield return StartCoroutine(fadeManager.FadeOut(fadeOutDuration));
            Debug.Log("페이드 아웃 완료. 씬을 로드합니다.");
        }
        else
        {
            Debug.LogWarning("FadeManager가 할당되지 않았습니다. 페이드 아웃 없이 씬을 바로 로드합니다.");
        }

        // 페이드 아웃이 끝나면 씬을 로드합니다.
        SceneManager.LoadScene(targetSceneName);
    }
}