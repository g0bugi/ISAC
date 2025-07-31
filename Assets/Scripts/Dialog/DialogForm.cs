using UnityEngine;

public class DialogForm : MonoBehaviour
{
    public int objectID;                // ex: NPC ID
    [TextArea(3, 5)]
    public string[] lines;              // 해당 NPC의 대사 목록
}
