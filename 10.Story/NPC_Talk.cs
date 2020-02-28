using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TalkBox
{
    EVENT,
    KEYWORD
}

public class NPC_Talk : MonoBehaviour
{
    private Transform tr;
    private TalkBox answerType;
    private int who;
    private Npc npc;

    private void Start()
    {
        tr = transform;
    }

    /*
        0 : 헤이즐      1:소니아
        2:트리스타      3:엘제어
        4:쿠치          5:우솔
        6:사무엘        7:비앙카
    */
    public void On(int num)
    {
        gameObject.SetActive(true);
        who = num;
        npc = NpcData.Instance.npcArray[who];

        SetData(TalkBox.EVENT);
    }

    //대화창의 구성요소들을 변경해줌.
    public GameObject box;
    private void SetData(TalkBox type)
    {
        if(answerType == TalkBox.EVENT)
        {

        }
        else//키워드 나열
        {
            
        }
    }

    public void Next()
    {
        
    }

    public void End()
    {
        Destroy(gameObject);
    }

    public void Click(int num)
    {
        if(answerType == TalkBox.EVENT)//이벤트시 이벤트 실행
        {

        }
        else //키워드 대화 시 키워드 선택
        {

        }
    }
}
