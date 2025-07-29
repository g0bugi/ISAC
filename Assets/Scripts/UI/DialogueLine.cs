using UnityEngine;

[System.Serializable] // Unity Inspector에서 보이도록 직렬화
public class DialogueLine
{
    public string characterName; // 캐릭터 이름
    [TextArea(3, 5)] // Inspector에서 여러 줄 입력 가능하게 함
    public string dialogueText;  // 대사 내용
}