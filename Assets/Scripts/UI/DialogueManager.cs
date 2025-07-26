using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // 대화창 UI
    public Text dialogueText;       // 대화 내용 텍스트
    public NpcController npcController; // 대화 끝나고 이동

    private string[] currentDialogue;
    private int currentDialogueIndex = 0;
    public bool IsDialogueActive { get; private set; } = false;

    public void StartDialogue(string[] dialogueLines)
    {
        currentDialogue = dialogueLines;
        currentDialogueIndex = 0;
        dialoguePanel.SetActive(true);
        IsDialogueActive = true;
        Debug.Log("대화 활성화");

        if (npcController != null)
        {
            npcController.SetSitting(true);
        }

    }

    public void DisplayNextSentence()
    {
        if (currentDialogueIndex >= currentDialogue.Length)
        {
            EndDialogue();
            return;
        }
        dialogueText.text = currentDialogue[currentDialogueIndex];
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