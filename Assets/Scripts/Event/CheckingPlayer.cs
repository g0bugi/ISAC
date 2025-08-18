using Unity.Mathematics.Geometry;
using UnityEngine;

public class CheckingPlayer : MonoBehaviour
{
    public GameObject player; // 플레이어
    public GameObject Nurse; //간호사 NPC 
    public float limit = 10; //제한 거리 지정
    public float TPposition = 5; // player가 강제적으로 위치하게 되는 거리
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DialogueManager.NurseTalkEnd == true) //간호사가 움직이는 중이라면
        {
            Debug.Log("체크 시작");
            check();
        }
    }
    void check()
    {
        float distance = Mathf.Sqrt(Mathf.Pow(player.transform.position.x -  Nurse.transform.position.x,2)+Mathf.Pow(player.transform.position.y - Nurse.transform.position.y,2));//player와 Nurse사이 거리
        if(distance > limit)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = Nurse.transform.position - Nurse.transform.forward * TPposition;
            Vector3 dir =Nurse.transform.position - player.transform.position;
            player.transform.forward = dir;

            if (cc != null) cc.enabled = true;
        }
    }
}
