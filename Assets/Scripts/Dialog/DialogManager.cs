using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 대화 관리를 총괄하는 메인 스크립트입니다.
public class DialogManager : MonoBehaviour
{
    // UI 요소 및 상태 변수들을 선언합니다.
    public TextMeshProUGUI dialogBox; // 대화 내용이 표시될 TextMeshPro UI
    GameObject scanObject; // 현재 상호작용 중인 게임 오브젝트
    public GameObject dialogPanel; // 대화창의 부모가 되는 패널 UI
    public bool isAction = false; // 현재 대화가 진행 중인지 여부를 나타내는 플래그
    public TalkContent talk; // 대화 데이터를 담고 있는 스크립터블 오브젝트 또는 컴포넌트
    public int talkIndex = 0; // 현재 대화의 진행도를 나타내는 인덱스

    // 게임이 시작될 때 대화창을 비활성화 상태로 초기화합니다.
    private void Start()
    {
        dialogPanel.SetActive(isAction);
    }

    // 외부(Interaction.cs)에서 호출되어 대화 프로세스를 시작하거나 제어합니다.
    public void Action(GameObject scanObj)
    {
        // 대화 시작을 위해 isAction 플래그를 true로 설정합니다.
        isAction = true;
        scanObject = scanObj;

        // 스캔된 오브젝트로부터 ID와 NPC 여부 데이터를 가져옵니다.
        ObjData objData = scanObj.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);

        // isAction 상태에 따라 대화창 UI를 활성화/비활성화합니다.
        dialogPanel.SetActive(isAction);
    }

    // 실제 대화 내용을 처리하고 UI에 표시하는 함수입니다.
    void Talk(int id, bool isNpc)
    {
        // 현재 대화 ID에 해당하는 모든 대화 내용이 끝났는지 확인합니다.
        if (talkIndex == talk.talkData[id].Length)
        {
            // 대화가 종료되면 isAction 플래그를 false로 설정하고 인덱스를 초기화합니다.
            isAction = false;
            talkIndex = 0;
            return; // 함수 실행을 종료합니다.
        }

        // 대상이 NPC인 경우에만 대화를 진행합니다.
        if (isNpc)
        {
            // TalkContent에서 현재 인덱스에 맞는 대사를 가져와 UI에 표시합니다.
            dialogBox.text = talk.GetTalk(id, talkIndex);
            // 다음 대사를 위해 인덱스를 1 증가시킵니다.
            talkIndex++;
        }
        else
        {
            // NPC가 아니면 대화를 진행하지 않고 함수를 종료합니다.
            return;
        }
    }
}
