using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // 대화창 UI
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;       // 대화 내용 텍스트
    public NpcController npcController; // 대화 끝나고 이동
    public float dialogueCameraPitch = 15f; // 카메라 각도

    private DialogueLine[] currentDialogueLines; 
    private int currentDialogueIndex = 0;
    public bool IsDialogueActive { get; private set; } = false;
    private bool waitingForNpcMovement = false;

    public Playermove playerMovement; // 움직임 제어

    public void StartDialogue(DialogueLine[] dialogueLines)
    {
        currentDialogueLines = dialogueLines;
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(true);
        IsDialogueActive = true;
        waitingForNpcMovement = false;
        Debug.Log("대화 활성화");

        if (npcController != null)
        {
            npcController.SetSitting(true);
        }

        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
            playerMovement.SetCameraPitchLock(true, dialogueCameraPitch);
        }

    }

    public void DisplayNextSentence()
    {
        if (waitingForNpcMovement) return;

        if (currentDialogueIndex >= currentDialogueLines.Length)
        {
            EndDialogue();
            return;
        }
         // 현재 대화 라인 가져오기
        DialogueLine currentLine = currentDialogueLines[currentDialogueIndex];

        // 이름 표시
        if (nameText != null) // 이름 텍스트 컴포넌트가 연결되어 있다면
        {
            nameText.text = currentLine.characterName;
        }
        
        // 대사 내용 표시
        dialogueText.text = currentLine.dialogueText;

        if (currentLine.triggerNpcMovement && npcController != null)
        {
            waitingForNpcMovement = true; // NPC 이동을 기다리는 중으로 설정
            Debug.Log("NPC 이동 트리거!");

            // NPC 이동 시작, 완료 시 호출될 콜백 함수 지정
            npcController.MoveToPosition(currentLine.targetMovePosition, currentLine.moveSpeed, OnNpcMovementComplete, currentLine.returnAfterMovement);
        }
        else
        {
            // NPC 이동이 없으면 바로 다음 대화 줄 인덱스 증가
            currentDialogueIndex++;
        }
    }
    
    private void OnNpcMovementComplete()
    {
        Debug.Log("DialogueManager가 NPC 이동 완료 신호를 받음.");
        waitingForNpcMovement = false; // 이동 대기 상태 해제
        currentDialogueIndex++; // NPC 이동 후 다음 대화 줄로 넘어감
        // 만약 이동 후 바로 다음 대사가 나오게 하려면 아래 줄의 주석을 해제
        DisplayNextSentence(); 
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        IsDialogueActive = false;
        waitingForNpcMovement = false;

        if (npcController != null)
        {
            npcController.SetSitting(false);
        }

        if (playerMovement != null)
        {
            playerMovement.SetCanMove(true);
            playerMovement.SetCameraPitchLock(false);
        }

        Debug.Log("대화 종료!");

    }

    void Update()
    {
        if (IsDialogueActive && Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 누르면
        {
            DisplayNextSentence();
        }
    }
}