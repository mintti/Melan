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
    public int[] skill; //사용자가 찍은 스킬.
    public bool[] usedSkill; //사용하는 스킬
    public int skillPoint; //남은 스킬 포인트.
    public int favor;
    public int day;
    public int stress;
    public int[] skinArr;

    //추후 Array로 바꾸기.
    public Sprite skin; //Unit.skins.closet[skinNum]으로 호출됨.

    //[XML]로드 이후, 스킬 탐색 함수를 통해 값이 지정된다.
    public int hp;
    public int power;
    public bool[,] tolerance;
    
    //Unit – team 탐색을 통해 지정.
    public bool teaming;
    
   

    //xml로드 값
    public Knight(int _num, string _name, int _job, int _level, int _exper, int[] _skill,
                  bool[] _usedSkill, int _skillPoint, int _favor, int _day, int _stress,
                  int[] _skinArr)
    {
        num = _num; name = _name; job = _job; level = _level; exper = _exper; skill = _skill;
        usedSkill = _usedSkill; skillPoint = _skillPoint; favor = _favor; day = _day; stress = _stress;
        skinArr = _skinArr;
    }

    public void Test_Stat(int _hp, int _power)
    {
        hp = _hp;
        power = _power;
    }
}

[System.Serializable]
public class Party
{
    public int dungeonNum;
    public int[] kngihtNum;
    public int[] hp;
    public int[] stress;
    public int day{get;set;}

    public Party(int _dNum, int[] _kNum, int[] _hp, int[] _stress, int day)
    {  
        dungeonNum = _dNum;
        kngihtNum = _kNum;
        hp = _hp;
        stress = _stress;

        DataController.InstanceUpdata();
    }
}
#endregion


public class UnitData : MonoBehaviour
{
    public SkinData skins;

    public List<Knight> knights = new List<Knight>(); //기사리스트
    public List<int> heroList = new List<int>(); //영웅 리스트
    public List<Party> partys = new List<Party>();

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
        Knight testKight = new Knight(0, "테스트용사", 1, 1, 0, new int[]{1,1,1,1,1},
                         new bool[]{true, true, true, true, false},
                         5, 2, 1, 0,
                         new int[]{0});
        testKight.Test_Stat(20, 10);
        knights.Add(testKight);
        SetSkin(0);

        //테스트 용사2
        testKight = new Knight(1, "테스트마법", 1, 1, 0, new int[]{1,1,1,1,1},
                         new bool[]{true, true, true, true, false},
                         5, 2, 1, 0,
                         new int[]{1});
        testKight.Test_Stat(10, 12);
        knights.Add(testKight);
        SetSkin(1);
        
        //테스트 용사3
        testKight = new Knight(2, "테스트도적", 1, 1, 0, new int[]{1,1,1,1,1},
                         new bool[]{true, true, true, true, false},
                         5, 2, 1, 0,
                         new int[]{2});
        testKight.Test_Stat(10, 8);
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

        int dNum = list[size - 1];
        int[] Karr = new int[size];
        int[] HParr = new int[size];
        int[] Sarr = new int[size];
        
        for(int i = 0 ; i < size; i++)
        {
            knights[list[i]].teaming = true;
            Karr[i] = list[i];
            HParr[i] = knights[list[i]].hp;
            Sarr[i] = knights[list[i]].stress;
        }

        partys.Add(new Party(dNum, Karr, HParr, Sarr, list[size]));
    }

    //해당 인덱스 찾기
    public int GetPartyIndex(int n)
    {
        return partys.FindIndex( i => i.dungeonNum == n);
    }
    
    public void RemoveTeam(int num)
    {

    }

}
