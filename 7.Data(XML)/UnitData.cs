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
    public Sprite skin; //Unit.skins.closet[skinNum]으로 호출됨.

    //[XML]로드 이후, 스킬 탐색 함수를 통해 값이 지정된다.
    public int hp;
    public int power;
    public bool[,] tolerance;
    
    //Unit – team 탐색을 통해 지정.
    public bool teaming = false;
    

    //xml로드 값
    public Knight(int _num, string _name, int _job, int _level, int _exper, int[] _skill,
                  bool[] _usedSkill, int _skillPoint, int _favor, int _day, int _stress,
                  Sprite _skin)
    {
        num = _num; name = _name; job = _job; level = _level; exper = _exper; skill = _skill;
        usedSkill = _usedSkill; skillPoint = _skillPoint; favor = _favor; day = _day; stress = _stress;
        skin = _skin;
    }
}
#endregion


public class UnitData : MonoBehaviour
{
    public SkinData skins;

    public List<Knight> knights = new List<Knight>(); //기사리스트
    public List<int> heroList = new List<int>(); //영웅 리스트
    private List<int[]> team = new List<int[]>();//팀
    
    

    public void Test_InsertData()
    {
        Knight testKight = new Knight(1, "테스트용사", 1, 1, 0, new int[]{1,1,1,1,1},
                         new bool[]{true, true, true, true, false},
                         5, 2, 1, 0,
                         skins.closet[0]);
                         
        knights.Add(testKight);
        //테스트 용사2
        testKight = new Knight(2, "테스트마법", 1, 1, 0, new int[]{1,1,1,1,1},
                         new bool[]{true, true, true, true, false},
                         5, 2, 1, 0,
                         skins.closet[1]);
        knights.Add(testKight);
        //테스트 용사3
        testKight = new Knight(3, "테스트도적", 1, 1, 0, new int[]{1,1,1,1,1},
                         new bool[]{true, true, true, true, false},
                         5, 2, 1, 0,
                         skins.closet[2]);
        knights.Add(testKight);
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

    public void AddTeam(int[] list)
    {
        for(int i = 0 ; i < list.Length ; i++)
        {
            //-1을 하는 이유는 knight는 1부터 저장된다.
            knights[list[i]-1].teaming = true;
            
        }

        team.Add(list);
    }

    public void RemoveTeam(int num)
    {

    }

}
