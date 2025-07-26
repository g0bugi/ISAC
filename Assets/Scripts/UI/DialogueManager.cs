using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // 대화창 UI
    public Text nameText;
    public Text dialogueText;       // 대화 내용 텍스트
    public NpcController npcController; // 대화 끝나고 이동

    private DialogueLine[] currentDialogueLines; 
    private int currentDialogueIndex = 0;
    public bool IsDialogueActive { get; private set; } = false;

    public Playermove playerMovement; // 움직임 제어

    public void StartDialogue(DialogueLine[] dialogueLines)
    {
        currentDialogueLines = dialogueLines;
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(true);
        IsDialogueActive = true;
        Debug.Log("대화 활성화");

        if (npcController != null)
        {
            npcController.SetSitting(true);
        }

        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

    }

    public void DisplayNextSentence()
    {
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

        currentDialogueIndex++;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        IsDialogueActive = false;

        if (npcController != null)
        {
            npcController.SetSitting(false);
        }

        if (playerMovement != null)
    {
        playerMovement.SetCanMove(true);
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