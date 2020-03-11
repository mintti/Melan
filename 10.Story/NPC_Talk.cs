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
        npcImg.sprite = ImageData.Instance.npc[who];
        npcNameText.text = npc.name;

        answerType = TalkBox.EVENT;
        SetData();
        SetTypingData();
    }

    //대화창의 구성요소들을 변경해줌.
    public Transform boxTr;
    public GameObject box;
    public Image npcImg;
    public Text npcNameText;
    
    private void SetData()
    {
        //기본세팅
        page = 0;
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
            for(int i =0 ; i< NpcData.Instance.npcTalk_Keyword.Length; i++)
            {
                if(!NpcData.Instance.npcTalk_Keyword[i].unlock) continue;
                CodeBox.AddChildInParent(boxTr, box).GetComponent<Npc_Talk_Box>().SetBox(transform, i, npc.Use_Keyword(i));
            }
        }
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Click(int num)
    {
        if(answerType == TalkBox.EVENT)//이벤트시 이벤트 실행
        {
            switch(num)
            {
                case 0 :
                    answerType = TalkBox.KEYWORD;
                    SetData();
                    break;
                case 1 :
                    break;
                case 2 :
                    break;
                case 3 :
                    break;
                case 4 :
                    break;
                default :
                    break;
            }
        }
        else //키워드 대화 시 키워드 선택
        {
            keywordNum  = num;
            page = 2;
            SetMent();
        }
    }
    
    public Text talkBox;
    /*
    해당부분 구현되도록 수정하기.
    */
    private TypingText typingText;
    private string mentType; //ment or mentarray
    
    private int keywordNum;
    private int KeywordNum{get{return npc.KeywordType(keywordNum) != 0 ? keywordNum : 9999;} set{keywordNum = value;}}
    private int mentNum;
    public int MentNum{get{return mentNum;}
        set{
            mentNum = value;
            int size = mentType == "ment" ? 1 : mentArray.ment.Length;
            if(mentNum >= size)
            {
                End();
                return;
            }
            Ment();
        }}

    private Ment ment;
    private MentArray mentArray;
    public void SetTypingData()
    {
        //ment = or mentarray = 지정
        typingText = transform.GetComponentInChildren<TypingText>();
        
        mentNum = 0;
        page = 0;

        talkingObj.SetActive(false);
        SetMent();
    }
    
    
    private Ment before;
    private void Ment()
    {
        talkingObj.SetActive(true);
        Ment main = mentType == "ment" ? ment : mentArray.ment[mentNum];
        //main멘트에 따라 화면구성 변경 
        typingText.SetData(transform, CheckMent(main.ment));
    }

    private void SetMent()
    {
        switch(page)
        {
            case 0 :
                mentType = "mentArray";
                mentArray = npc.mentList.greeting_Front_Favor[npc.level];
                break;
            case 1 :
                mentType = "ment";
                ment = npc.mentList.greeting;
                break;
            case 2 : //Keyword
                mentType = "mentArray";
                mentArray = npc.mentList.keyword.Find(k => k.num == KeywordNum).mentArray;
                break;
            case 3 : //Keyword Reaction
                mentType = "ment";
                npc.Event_Keyword(keywordNum);//호감도 증감
                int type = npc.KeywordType(keywordNum);//리액션 넘버
                ment = type == 0 ? npc.mentList.keyword_None :
                       type == 1 ? npc.mentList.keyword_Like :
                       type == 2 ? npc.mentList.keyword_Nomal :
                                   npc.mentList.keyword_Hate;
                break;
            default :
                break;
        }
        MentNum = 0;
    }
    

    public void Next()
    {
        MentNum++;
    }
    
    private void End()
    {
        talkingObj.SetActive(false);
        int[] nextMent = new int[5]{1, -1, 3, 1, 1};
        page = nextMent[page];

        if(page == -1)
        {
            answerType = TalkBox.EVENT;
            SetData();
            return;
        }
        SetMent();
    }

    public GameObject talkingObj;
    private bool isTalking;


    private string CheckMent(string text)
    {
        //본인의 이름
        string correctMent = text.Replace("$0", npc.name);
        //주인공의 이름
        correctMent = correctMent.Replace("#0", NpcData.Instance.npcArray[0].name);
        //키워드의 이름
        correctMent = correctMent.Replace("^0", NpcData.Instance.npcTalk_Keyword[keywordNum].name );

        return correctMent;
    }
}
