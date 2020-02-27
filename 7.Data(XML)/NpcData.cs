using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc
{
    public string name;
    public int favor;
    public Npc(){}
    public Npc(string n, int f)
    {
        name = n; favor = f;
    }
    
    //성향적 키워드
    private List<int> keyword_Like = new List<int>();
    private List<int> keyword_Nomal = new List<int>();
    private List<int> keyword_Hate = new List<int>();

    //사용했던 키워드. 사용했던 경우 효과를 볼수 있음. 
    public List<int> used_Keyword = new List<int>();
    
    public List<int> use_Action = new List<int>();
    //키워드 대화시, 호감도 증가 혹은 감소
    public void Event_Keyword(int num)
    {
        if(keyword_Like.Contains(num));
        else if(keyword_Nomal.Contains(num));
        else if(keyword_Hate.Contains(num));
        else;

        if(!used_Keyword.Contains(num)) used_Keyword.Add(num);
    }
    public void Event_Action(int num)
    {
        use_Action.Add(num);        
    }

    public void NextDay()
    {
        use_Action.Clear();    
    }
    
}

public class NpcData : MonoBehaviour
{
    public Npc[] npcArray;

    public void SetNpc(bool isNew)
    {
        //is new해서 로드할지 말지 설정.
        npcArray = new Npc[5]{
            new Npc("세레나", 0),
            new Npc("npc2", 0),
            new Npc("npc3", 0),
            new Npc("npc4", 0),
            new Npc("npc5", 0)};
    }
}
