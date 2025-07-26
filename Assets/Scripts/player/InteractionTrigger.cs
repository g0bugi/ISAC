// InteractionTrigger.cs (씬 전환 없는 버전)
using UnityEngine;
using System.Collections; // Coroutine을 위해 필요

public class InteractionTrigger : MonoBehaviour
{
    public FadeManager fadeManager;
    public DialogueManager dialogueManager;
    public DialogueLine[] dialogueLines; // 이 트리거에서 시작할 대화 내용

    // 플레이어 위치 수정을 위한 변수
    public Transform playerTransform; 
    public Vector3 targetPlayerPosition; 
    public Quaternion targetPlayerRotation;

    public Transform npcTransform; 
    public Vector3 targetNpcPosition; 
    public Quaternion targetNpcRotation; 
     public Vector3 targetNpcWalkPosition; // 대화 종료 후 NPC가 걸어갈 최종 목표 위치

    public float delayBetweenFades = 1.0f;

    private bool hasTriggered = false; // 한 번만 트리거되도록

    private void OnTriggerEnter(Collider other)
    {
        // "Player" 태그를 가진 오브젝트가 진입했고, 아직 트리거되지 않았다면
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // 한 번만 실행되도록 플래그 설정
            // 플레이어 Transform을 자동으로 가져오려면
            if (playerTransform == null)
            {
                playerTransform = other.transform;
            }
            StartCoroutine(FadeModifyPlayerAndStartDialogue());
        }
    }

    private IEnumerator FadeModifyPlayerAndStartDialogue()
    {

        yield return StartCoroutine(fadeManager.FadeOut(1.0f)); // 1초 동안 페이드 아웃

        if (playerTransform != null)
        {
            playerTransform.position = targetPlayerPosition;
            playerTransform.rotation = targetPlayerRotation;
            Debug.Log("플레이어 위치 및 방향 수정 완료!");
        }

        if (npcTransform != null)
        {
            npcTransform.position = targetNpcPosition;
            npcTransform.rotation = targetNpcRotation;
            Debug.Log("NPC 위치 및 방향 수정 완료!");
        }

        yield return new WaitForSeconds(delayBetweenFades);

        dialogueManager.StartDialogue(dialogueLines);

        yield return StartCoroutine(fadeManager.FadeIn(1.0f)); // 1초 동안 페이드 인


        

        // DialogueManager의 IsDialogueActive 속성을 사용하여 대화 종료를 감지
        yield return new WaitUntil(() => !dialogueManager.IsDialogueActive);
        Debug.Log("InteractionTrigger: 대화 종료 감지.");

        //대화가 끝나면 NPC가 걸어서 목표 위치로 이동
        if (dialogueManager.npcController != null) // DialogueManager에 연결된 NpcController 사용
        {
            dialogueManager.npcController.StartWalkingTo(targetNpcWalkPosition);
        }


    
    }
}