using UnityEngine;

public class DialogForm : MonoBehaviour
{
    public int objectID;                // ex: NPC ID
    [TextArea(3, 5)]
    public string[] lines;              // �ش� NPC�� ��� ���
}
