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
    private TalkBox answerType;
    private int who;
    private Npc npc;
    private int page;
    
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

        answerType = TalkBox.EVENT;
        SetData();
    }

    //대화창의 구성요소들을 변경해줌.
    public Transform boxTr;
    public GameObject box;
    public Image npcImg;
    private void SetData()
    {
        //기본세팅
        page = 0;
        npcImg.sprite = ImageData.Instance.npc[who];
        //선택지 세팅
        CodeBox.ClearList(boxTr);
        if(answerType == TalkBox.EVENT)
        {
            for(int i = 0 ; i < npc.have_Event.Count; i++)
            {
                int num = npc.have_Event[i];

                CodeBox.AddChildInParent(boxTr, box).GetComponent<Npc_Talk_Box>().SetBox(transform, num, NpcData.Instance.npcTalk_Event[num]);
            }
        }
        else//키워드 나열
        {
            
        }
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
