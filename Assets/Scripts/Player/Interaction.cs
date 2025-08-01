using UnityEngine;

// 플레이어의 상호작용을 처리하는 스크립트입니다. (예: NPC와의 대화 시작)
public class Interaction : MonoBehaviour
{
    // 필요한 컴포넌트 및 변수들을 선언합니다.
    public DialogManager manager; // 대화 관리를 위한 DialogManager 참조
    RaycastHit hit; // Raycast의 충돌 정보를 저장할 변수
    float maxdistance = 15f; // Raycast가 도달할 수 있는 최대 거리
    public LayerMask mask; // Raycast가 감지할 특정 레이어 (예: "NPC" 레이어)
    public bool IsInteraction = false;
    Playermove move;

    // MonoBehaviour의 생명주기 함수 Start (현재는 내용 없음)
    void Start()
    {
       
    }

    // 매 프레임마다 호출되는 MonoBehaviour의 생명주기 함수입니다.
    void Update()
    {
        // 'R' 키가 눌렸는지 확인합니다.
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            // 플레이어의 정면으로 Raycast를 발사하여 상호작용 가능한 오브젝트를 찾습니다.
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxdistance, mask))
            {
                Playermove move = GetComponent<Playermove>();
                move.canMove = false;
                IsInteraction = true;
                Debug.Log(hit.collider.name);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("NPC"))
                    {
                    
                    // Raycast에 감지된 오브젝트와 상호작용을 시작하기 위해 DialogManager의 Action 함수를 호출합니다.
                    manager.Action(hit.collider.gameObject);
                    move.canMove = true;
                }
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Object"))
                {
                    GameObject scanobj = hit.collider.gameObject;
                    scanobj.GetComponent<IInteractiable>().Action();
                    move.canMove = true;
                }
                
            }
        }
    }
}
