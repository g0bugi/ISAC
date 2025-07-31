using UnityEngine;

[System.Serializable]
// 게임 오브젝트에 대한 기본 데이터를 저장하는 컴포넌트입니다.
// 주로 상호작용 가능한 오브젝트(NPC, 아이템 등)에 부착하여 사용합니다.
public class ObjData : MonoBehaviour
{
    // 오브젝트의 고유 식별자(ID)입니다. 대화 내용 등을 구분하는 데 사용됩니다.
    public int id;

    // 이 오브젝트가 NPC인지 여부를 나타내는 플래그입니다.
    public bool isNpc;

    public string[] name;
    public string[] Text;
}
