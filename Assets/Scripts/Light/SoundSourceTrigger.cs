using UnityEngine;
using System.Collections;

public class SoundSourceTrigger : MonoBehaviour
{
    public AudioSource audioSource; // 이 오브젝트의 AudioSource 컴포넌트
    private SoundLight rippleManager; // 파장 효과 매니저 참조

    public float initialDelay = 0f; // 게임 시작 후 첫 파장이 나타나기까지의 지연 시간
    public float repeatInterval = 1.0f; // 파장 효과가 반복되는 간격 (초)
    public Sprite specificRippleSprite; // 파장 이미지  

    void Start()
    {

        // 여기서는 FindObjectOfType을 사용합니다. (씬에 하나만 있다고 가정)
        rippleManager = FindObjectOfType<SoundLight>();
        if (rippleManager == null)
        {
            Debug.LogError("씬에서 SoundLight를 찾을 수 없습니다.");
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource 컴포넌트가 없습니다.");
            }
        }
        StartCoroutine(RepeatRipple());
    }

    IEnumerator RepeatRipple()
    {
        // 초기 지연 시간만큼 기다립니다.
        if (initialDelay > 0)
        {
            yield return new WaitForSeconds(initialDelay);
        }

        // 무한 루프를 돌면서 소리와 파장 효과를 반복적으로 생성합니다.
        while (true)
        {
            PlayRipple();

            yield return new WaitForSeconds(repeatInterval);
        }
    }

    // 소리가 재생될 때 이 함수를 호출합니다.
    // 예를 들어, 버튼 클릭 시, 특정 이벤트 발생 시 등
    public void PlayRipple()
    {
        if (audioSource != null)
        {


        }
        if (rippleManager != null)
            {
                // 현재 오브젝트의 위치를 파장 효과 매니저에게 전달
                rippleManager.PlayRippleEffect(transform.position, audioSource.maxDistance, specificRippleSprite);
            }
    }


}