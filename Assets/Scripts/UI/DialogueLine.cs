using UnityEngine;

[System.Serializable] // Unity Inspector에서 보이도록 직렬화
public class DialogueLine
{
    public string characterName; // 캐릭터 이름
    [TextArea(3, 5)] // Inspector에서 여러 줄 입력 가능하게 함
    public string dialogueText;  // 대사 내용

    public bool triggerNpcMovement; // 이 대화 줄 다음에 NPC 이동을 트리거할지 여부
    public Vector3 targetMovePosition; // NPC가 이동할 목표 위치
    public float moveSpeed = 1.5f; // NPC 이동 속도
    public bool returnAfterMovement; // 이동 후 다시 원래 위치로 돌아올지 여부
}