using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Npc
{
    public int num;
    public string name;
    public int favor;
    public int Favor
    {
        get{return favor;}
        set{
            favor = value;
            level = favor < 20 ? 0 : favor < 50 ? 1 : favor < 100 ? 2 : 3;
        }
    }
    public int level;
    private bool unlock;
    public bool Unlock{get{return unlock;}set{unlock = value; if(unlock)LoadMent();}}

    public Npc(){}
    //초기화 데이타(고정)
    public Npc(int _num, string n, List<int> h_e, List<int> k_l, List<int> k_n, List<int> k_h)
    {
        num = _num;
        name = n;
        have_Event = h_e;
        keyword_Like = k_l;
        keyword_Nomal = k_n;
        keyword_Hate = k_h;
    }
    //PlayerData Load
    public void SetData(int f, bool u)
    {
        Favor = f;
        Unlock = u;
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
    public void Event_Keyword(int num)
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
    }
    //0 : 성향비교
    public int KeywordType(int num)
    {
        if(keyword_Like.Contains(num))  return 1;
        if(keyword_Nomal.Contains(num)) return 2;
        if(keyword_Hate.Contains(num)) return 3;
        return 0;
    }
    public int Use_Keyword(int num)
    {
        int result = KeywordType(num);
        
        return used_Keyword.Contains(num) ? result : 0;
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
    

    public MentList mentList;
    public void LoadMent()
    {
        mentList = DataController.Instance.LoadXml_Npc_Ment(NpcData.Instance.npcName[num]);
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

public class MentList
{
    public Ment greeting;
    public MentArray[] greeting_Front_Favor;
    
    public Ment keyword_None;
    public Ment keyword_Like;
    public Ment keyword_Nomal;
    public Ment keyword_Hate;
    public List<MentArray_Keyword> keyword = new List<MentArray_Keyword>();
}

public class MentArray_Keyword
{
    public int num;
    public MentArray mentArray;

    public MentArray_Keyword(int n, MentArray ma)
    {
        num = n; mentArray = ma;
    }
}
public class MentArray
{
    public Ment[] ment;
    
    public MentArray(Ment[] m)
    {
        ment = m;
    }
}
public class Ment
{
    public string ment;
    public int face;
    public bool mouseMove;
    public int deco;

    public Ment(string m, int f, int d, bool mou)
    {
        ment = m; face = f; deco = d; mouseMove = mou;
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
    public string[] npcName;

    public void SetData()
    {
        npcName = new string[8]{"heizle", "sonia", "trista", "alger", "cuchi", "usol" , "samuel", "vianke"};
    }
}
