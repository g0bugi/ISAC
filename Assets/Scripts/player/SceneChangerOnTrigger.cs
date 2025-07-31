using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerOnTrigger : MonoBehaviour
{
    public string targetSceneName;
    public string targetTag = "Player"; // 여기에 특정 오브젝트의 태그를 입력합니다. (기본값: Player)

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 'targetTag'에 설정된 태그와 일치하는지 확인합니다.
        if (other.CompareTag(targetTag)) 
        {
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                SceneManager.LoadScene(targetSceneName);
            }
            else
            {
                Debug.LogWarning("씬 이름이 설정되지 않았습니다. Inspector에서 'Target Scene Name'을 설정해주세요.");
            }
        }
        else
        {
             Debug.Log($"태그가 '{targetTag}'인 오브젝트만 씬을 변경할 수 있습니다. 현재 충돌한 오브젝트의 태그: {other.tag}");
            // 특정 태그의 오브젝트가 아닐 경우 아무것도 하지 않습니다. (필요하다면 위 주석을 해제하여 디버그 메시지를 볼 수 있습니다.)
        }
    }
}