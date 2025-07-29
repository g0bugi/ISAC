using System.Collections.Generic;
using UnityEngine;

// 게임 내 모든 대화 데이터를 관리하는 스크립트입니다.
public class TalkContent : MonoBehaviour
{
    // 대화 데이터를 저장하는 Dictionary입니다. Key는 오브젝트 ID(int), Value는 대화 내용(string[])입니다.
    public Dictionary<int, string[]> talkData;

    // 스크립트가 활성화될 때 한 번 호출되는 Awake 함수입니다.
    private void Awake()
    {
        // talkData 딕셔너리를 초기화합니다.
        talkData = new Dictionary<int, string[]>();
        talkData.Clear(); // 만약을 위해 딕셔너리를 비웁니다.

        // 대화 데이터를 생성하고 딕셔너리에 추가하는 함수를 호출합니다.
        GenerateData();
    }

    // 실제 대화 데이터를 생성하는 함수입니다.
    // 나중에는 외부 파일(CSV, JSON 등)에서 읽어오는 방식으로 확장할 수 있습니다.
    void GenerateData()
    { 
        // ID가 1000인 NPC의 대화 내용을 추가합니다.
        talkData.Add(1000, new string[] { "안녕 나는 기타맨", "요즘 날씨가 좋지 않아?", "무슨 일로 나에게 말을 걸었지?" });
    }

    // 주어진 ID와 대화 인덱스에 해당하는 대화 내용을 반환하는 함수입니다.
    public string GetTalk(int id, int talkIndex)
    {
        // 딕셔너리에서 ID와 인덱스에 맞는 대사를 찾아 반환합니다.
        if (talkData.ContainsKey(id) && talkIndex < talkData[id].Length)
        {
            return talkData[id][talkIndex];
        }
        else
        {
            // 요청한 데이터가 없는 경우, 오류를 방지하기 위해 null이나 기본 메시지를 반환합니다.
            return "...";
        }
    }
}