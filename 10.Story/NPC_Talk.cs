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
    private void Awake() {
        SetMentDelegate();
    }

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
        npcLevelText.text = string.Format("LV.{0}", npc.level);

        answerType = TalkBox.EVENT;
        SetData();
        SetTypingData();
    }

    //대화창의 구성요소들을 변경해줌.
    public Transform boxTr;
    public GameObject box;
    public Image npcImg;
    public Text npcNameText;
    public Text npcLevelText;
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
                CodeBox.AddChildInParent(boxTr, box).GetComponent<Npc_Talk_Box>().SetBox(transform, i, npc.Used_Keyword_Icon(i));
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
    
    #region Ment 지정영역
    public Text talkBox;
    private TypingText typingText;
    private string mentType; //ment or mentarray
    public GameObject talkingObj;//

    private int keywordNum;
    private int KeywordNum{get{return npc.KeywordType(keywordNum) == 4 ? 9999 : keywordNum;} set{keywordNum = value;}}
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
    
    private Ment before;//추 후 비교문
    private void Ment()
    {
        talkingObj.SetActive(true);
        Ment main = mentType == "ment" ? ment : mentArray.ment[mentNum];
        //main멘트에 따라 화면구성 변경 
        typingText.SetData(transform, CheckMent(main.ment));
    }

    Delegate[] MentDelegate; //Awake에서 아래 설정댐

    private void SetMentDelegate()
    {
        MentDelegate = new Delegate[4];
        MentDelegate[0] = Greeting_Front_Favor;
        MentDelegate[1] = Greeting;
        MentDelegate[2] = Keyword_Answer;
        MentDelegate[3] = Keyword_Reaction;
    }
    
    private void SetMent()
    {
        MentDelegate[page]();
        MentNum = 0;
    }
    //호감도에 따른 첫 안내인사
    private void Greeting_Front_Favor()
    {
        mentType = "mentArray";
        mentArray = npc.mentList.greeting_Front_Favor[npc.level];
    }
    //목적을 묻는 안내인사
    private void Greeting()
    {
        mentType = "ment";
        ment = npc.mentList.greeting;
    }
    //키워드에 따른 대답
    private void Keyword_Answer()
    {
        mentType = "mentArray";
        mentArray = npc.mentList.keyword.Find(k => k.num == KeywordNum).mentArray;
    }
    //키워드에 따른 반응
    private void Keyword_Reaction()
    {
        mentType = "ment";
        npc.Event_Keyword(keywordNum);//호감도 증감
        
        int type = npc.KeywordType(keywordNum);//리액션 넘버
        ment = type == 1 ? npc.mentList.keyword_Like :
               type == 2 ? npc.mentList.keyword_Nomal :
               type == 3 ? npc.mentList.keyword_Hate :
                           npc.mentList.keyword_None;
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
    #endregion
}
