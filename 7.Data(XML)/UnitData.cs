using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#region Unit, Knight Class Info

[System.Serializable]
public class Knight
{
    public int num; //★key
    public string name;//사용자 지정 이름.
    public int job;
    public int level;
    public int exper; //경험치
    
    public int[] usedSkill; //사용하는 스킬
    public int skillPoint; //남은 스킬 포인트.
    public int favor;
    public int day;
    public int hp;
    public int stress;
    public int[] skinArr;

    public List<int> uni;
    //추후 Array로 바꾸기.
    public Sprite skin; //Unit.skins.closet[skinNum]으로 호출됨.

    //[XML]로드 이후, 스킬 탐색 함수를 통해 값이 지정된다.
    public int maxHp;
    public int power;
    public int speed;
    public bool[,] tolerance;
    
    //Unit – team 탐색을 통해 지정.
    public bool teaming;
    
   

    //xml로드 값
    public Knight(int _num, string _name, int _job, int _level, int _exper,
                  int[] _usedSkill, int _skillPoint, int _favor, int _day, int _hp, int _stress,
                  int[] _skinArr)
    {
        num = _num; name = _name; job = _job; level = _level; exper = _exper;
        usedSkill = _usedSkill; skillPoint = _skillPoint; favor = _favor; day = _day; hp = _hp; stress = _stress;
        skinArr = _skinArr;
    }

    public void Test_Stat(int _hp, int _power, int _speed)
    {
        hp = _hp;
        power = _power;
        speed = _speed;
    }
}
#endregion

#region Party Class Info

[System.Serializable]
public class Party
{
    public int dungeonNum;
    public int[] k;
    public KnightState[] knightStates;

    public int day{get;set;}

    public Party()
    {
        
    }
    public Party(int _d, int[] _kNum)
    {  
        dungeonNum = _d;
        
        int size = _kNum.Length;
        this.k = new int[size];
        knightStates = new KnightState[size];
        for(int i = 0; i< size; i++)
        {
            Knight k = DataController.Instance.unit.knights[_kNum[i]];
            this.k[i] = _kNum[i];
            knightStates[i] = new KnightState(k);
        }
        day = CodeBox.DungoenReturn(dungeonNum).day;
    }

    //Xml 불러오기
    public void LoadParty(int _d, int[] _k, KnightState[] _ks, int _day)
    {
        dungeonNum = _d;
        int size = _k.Length;

        k = new int[size];
        k = _k;
        knightStates = new KnightState[size];
        knightStates = _ks;

        day = _day;
    }

    //Day갱신.
    public void NextDay()
    {
        day--;
    }
}

//전투 시 Knight정보
public class KnightState
{
    public Knight k;
    public State s;

    public KnightState(Knight _k)
    {
        k = _k;
        s = new State(k.hp, k.power, k.speed, k.stress, k.uni, LifeType.K);
    }
}
#endregion


public class UnitData : MonoBehaviour
{
    public SkinData skins;

    public List<Knight> knights = new List<Knight>(); //기사리스트
    public List<int> heroList = new List<int>(); //영웅 리스트
    public List<Party> partys = new List<Party>();

    /*
        기본스텟 : hp/power/speed 순
        0.전사  1.마법사  2.시프
                 */
    public int[,,] stateList = new int[3,5,3]
    {
        {{20, 10, 15}, {35, 15, 15}, {40, 20, 20}, {50, 25, 20}, {60, 30, 20}},
        {{10, 20, 5}, {15, 15, 15}, {40, 20, 20}, {50, 25, 20}, {60, 30, 20}},
        {{10, 5, 20}, {15, 10, 25}, {20, 20, 30}, {25, 25, 40}, {30, 30, 50}}
    };


    //Knight - SkinArr 탐색 후 Skin에 매칭시킴.
    public void SetSkin(int num)
    {
        int i = knights.FindIndex(o => o.num == num);

        knights[i].skin = skins.closet[knights[i].skinArr[0]];
        //추후 array탐색형식으로 변경.
        /*
        for(int j = 0; j < 1; j++)
        {

        }
         */
    }


    public void Test_InsertData()
    {
        Knight testKight = new Knight(0, "테스트용사", 0, 1, 0,
                         new int[]{0,1,2,3},
                         5, 2, 1, 20, 0,
                         new int[]{0});
        testKight.Test_Stat(20, 10, 15);
        knights.Add(testKight);
        SetSkin(0);

        //테스트 용사2
        testKight = new Knight(1, "테스트마법", 1, 1, 0,
                         new int[]{0,1,2,3},
                         5, 2, 1, 10, 0,
                         new int[]{1});
        testKight.Test_Stat(10, 20, 5);
        knights.Add(testKight);
        SetSkin(1);
        
        //테스트 용사3
        testKight = new Knight(2, "테스트도적", 2, 1, 0,
                         new int[]{0,1,2,3},
                         5, 2, 1, 10, 0,
                         new int[]{2});
        testKight.Test_Stat(10, 5, 20);
        knights.Add(testKight);
        SetSkin(2);
    }

    public void SetData(Knight k)
    {
        
    }
    #region 데이터세팅.추후 설정.
    //데이터 load 함수. 로직참고.
    void AdminDataLoad(){
        //Xml load

        //Data Setting
        /* 
        foreach(knight k in unit.knights)
        {
            //영웅을 관리 할 떄, 추가 탐색을 줄이기 위해 미리 List에 넣어둔다.
            if(k.job >= 100) 
                unit.heroList.Add(k.num);

            //스킬탐색
        }
        */
    }
    void SkillSearching()
    {

    }
    #endregion

    public void AddParty(int[] list) //list[0~n : Knight, n +1 : dungeon]
    {
        int size = list.Length -1;
        int[] Karr = new int[size];
        
        for(int i = 0 ; i < size; i++)
        {
            knights[list[i]].teaming = true;
            Karr[i] = list[i];
        }
        
        partys.Add(new Party(list[size], Karr));
    }

    //해당 인덱스 찾기
    public int GetPartyIndex(int n)
    {
        return partys.FindIndex( i => i.dungeonNum == n);
    }
    
    public void RemoveTeam(int num)
    {

    }



    
    
    //랜덤 용병생성
    public void CreateRandomKnight(int cnt)
    {
        Knight[] knightList = new Knight[cnt];
        for(int i = 0; i < cnt; i ++)
        {


        }
    }

}
