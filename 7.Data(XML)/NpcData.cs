using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc
{
    public string name;
    public int favor;
    
    public Npc(){}
    //초기화 데이타
    public Npc(string n, List<int> h_e, List<int> k_l, List<int> k_n, List<int> k_h)
    {
        name = n;
        have_Event = h_e;
        keyword_Like = k_l;
        keyword_Nomal = k_n;
        keyword_Hate = k_h;
    }
    //PlayerData Load
    public void SetData()
    {

    }

    //<Fix>성향적 키워드
    private List<int> keyword_Like = new List<int>();
    private List<int> keyword_Nomal = new List<int>();
    private List<int> keyword_Hate = new List<int>();
    //<Fix>가지는 Event
    public List<int> have_Event = new List<int>();
    
    //<Load>사용했던 키워드. 사용했던 경우 효과를 볼수 있음. 
    public List<int> used_Keyword = new List<int>();
    //하루하루 초기화
    public List<int> use_Event = new List<int>();

    //키워드 대화시, 호감도 증가 혹은 감소
    public int Event_Keyword(int num)
    {
        int type = KeywordType(num);
        switch(type)
        {
            case 0 : 
                used_Keyword.Add(num);
                Event_Keyword(num);
                break;
            case 1 : //Like
                favor += 3;
                break;
            case 2 : //Nomal
                favor += 1;
                break;
            case 3 : //hate
                favor -= 3;
                break;
            default ://none?
                break;    
        }
        Debug.Log(type + "실행");
        return type;
    }
    //0 : 성향비교
    public int KeywordType(int num)
    {
        if(!used_Keyword.Contains(num)) return 0;

        if(keyword_Like.Contains(num))  return 1;
        if(keyword_Nomal.Contains(num)) return 2;
        if(keyword_Hate.Contains(num)) return 3;
        return 4;
    }
    // 1_0 : 사용 이벤트에 추가 후 재비교
    public void Event_Action(int num)
    {
        use_Event.Add(num);        
    }

    

    public void NextDay()
    {
        use_Event.Clear();    
    }
    
}
public class Keyword
{
    public string name;
    public bool unlock;

    public Keyword(){}
    public Keyword(string n, bool u)
    {
        name = n; unlock = u;
    }
}

public class NpcData : MonoBehaviour
{
    private static NpcData _instance = null;
    public static NpcData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(NpcData)) as NpcData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active NpcData object");
                }
            }
            return _instance;
        }
    }

    public Npc[] npcArray{get;set;}
    public string[] npcTalk_Event{get;set;}

    public Keyword[] npcTalk_Keyword;
    
}
