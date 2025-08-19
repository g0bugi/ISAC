using Unity.Mathematics.Geometry;
using UnityEngine;
using System.Collections;

public class CheckingPlayer : MonoBehaviour
{
    public GameObject player; // �÷��̾�
    public GameObject Nurse; //��ȣ�� NPC 
    public float limit = 10; //���� �Ÿ� ����
    public float TPposition = 5; // player�� ���������� ��ġ�ϰ� �Ǵ� �Ÿ�

    public Playermove playerMoveScript;
    public NpcController NurseMoveScript;
    public DialogueManager dialogueManager;
    public DialogueLine[] scoldingDialogue;
    public FadeManager fadeEffect;

    private bool isScolding = false; // 대화 진행중?

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.NurseTalkEnd == true && !isScolding) //��ȣ�簡 �����̴� ���̶��
        {
            Debug.Log("체크 시작");
            check();
        }
    }
    void check()
    {
        float distance = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - Nurse.transform.position.x, 2) + Mathf.Pow(player.transform.position.y - Nurse.transform.position.y, 2));//player�� Nurse���� �Ÿ�
        if (distance > limit)
        {   
            StartCoroutine(TeleportAndScold());
        }
    }
    private IEnumerator TeleportAndScold()
    {
        // ✨대화 시작 상태로 변경
        isScolding = true;

        if (playerMoveScript != null) playerMoveScript.SetCanMove(false);
        if (NurseMoveScript != null) NurseMoveScript.SetCanMove(false); // 간호사도 멈춤

        yield return StartCoroutine(fadeEffect.FadeOut(0.5f));

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = Nurse.transform.position - Nurse.transform.forward * TPposition;
        Vector3 dir = Nurse.transform.position - player.transform.position;
        player.transform.forward = dir;



        yield return StartCoroutine(fadeEffect.FadeIn(0.5f));


        if (dialogueManager != null && scoldingDialogue.Length > 0)
        {
            dialogueManager.StartDialogue(scoldingDialogue);

            // 대화가 끝나면 다시 움직임을 활성화
            // DialogueManager 스크립트에서 호출해야 함
        }
        if (cc != null) cc.enabled = true;
    }
    
    // DialogueManager에서 대화가 끝났을 때 이 함수를 호출해야 합니다.
    public void DialogueEnd()
    {
        isScolding = false;
        if (playerMoveScript != null) playerMoveScript.SetCanMove(true);
        if (NurseMoveScript != null) NurseMoveScript.SetCanMove(true);
    }
}
