﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Unit, Knight Class Info

[System.Serializable]
public class Knight
{
    public int num; //★key
    public string name;//사용자 지정 이름.
    public int job;
    public int level;
    public int exper; //경험치
    
    public int[] skill; //보유한 스킬
    public int favor;
    public int day;

    public bool isAlive;
    public int hp;
    public int stress;
    
    public int[] uni;
    //추후 Array로 바꾸기.
    public int[] skinArr;
    public Skin skin; //Unit.skins.closet[skinNum]으로 호출됨.

    //[XML]로드 이후, 스킬 탐색 함수를 통해 값이 지정된다.
    public int maxHp;//job[level] 에 의한
    public int power;
    public int speed;
    public bool[,] tolerance; //uni에의한
    
    //Unit – team 탐색을 통해 지정.
    public bool teaming;
    
   

    //xml로드 값
    public Knight(int _num, string _name, int _job, int _level, int _exper,
                  int[] _skill, int _favor, int _day, int[] _skinArr, 
                  bool _isAlive, int _hp, int _stress, int[] _uni)
    {
        num = _num; name = _name; job = _job; level = _level; exper = _exper;
        skill = _skill; favor = _favor; day = _day;  skinArr = _skinArr;
        isAlive = _isAlive; hp = _hp; stress = _stress;  uni = _uni;

        skin = new Skin(skinArr);

        GetState();
    }

    public void GetState()
    {
        int[,,] stateList = UnitData.Instance.stateList; 
        maxHp = stateList[job, level-1 , 0];
        power = stateList[job, level-1, 1];
        speed = stateList[job, level-1, 2];
    }
}
[System.Serializable]
public class RandomKnight
{
    public string name;
    public int job;
    public int level;
    public int[] skinNum = new int[6];
    public int[] skill;

    public bool isEmploy;

    public RandomKnight(bool b)
    {
        if(b)
        {
           CreateRandomKnight(); 
        }
    }
    public RandomKnight(string _name, int _job, int _level, bool _employ, int[] _skinNum)
    {
        name = _name;
        job = _job;
        level = _level;
        isEmploy = _employ;
        skinNum = _skinNum;
    }

    public void CreateRandomKnight()
    {
        /*
        player UnitLevel별 허용 직업군
        1. 0~2
        2. 3~5
        3. 6~8
        */
        int playerLevel = PlayerData.Instance.Level;
        
        name = "테스트";
        job = Random.Range(0, playerLevel == 0 ? 3 : playerLevel == 1 ? 6 : 9);
        level = Random.Range(1, playerLevel + 2); 

        skinNum = SkinData.Instance.RandomSkin();
        isEmploy = false;
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
    public Party(int _d, int[] _kNum, int _day)
    {  
        dungeonNum = _d;
        
        int size = _kNum.Length;
        this.k = new int[size];
        knightStates = new KnightState[size];
        for(int i = 0; i< size; i++)
        {
            this.k[i] = _kNum[i];

            Knight k = UnitData.Instance.knights[_kNum[i]];
            k.teaming = true;
            knightStates[i] = new KnightState(k);
        }
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
    private static UnitData _instance = null;

    public static UnitData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(UnitData)) as UnitData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active UnitData object");
                }
            }
            return _instance;
        }
    }

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
        {{15, 20, 5}, {25, 15, 15}, {40, 20, 20}, {50, 25, 20}, {60, 30, 20}},
        {{15, 5, 20}, {20, 10, 25}, {20, 20, 30}, {25, 25, 40}, {30, 30, 50}}
    };
    public int[] knightPay = new int[5]
    {10, 20, 30, 40, 50};

    public List<int> useElementJob = new List<int>(){1};

    
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
            Karr[i] = list[i];
            
        int dungeon = list[size];
        int day = CodeBox.DungoenReturn(dungeon).day;

        Party p = new Party(dungeon, Karr, day);
        partys.Add(p);
        DataController.Instance.AddParty(p);
        
    }

    //해당 인덱스 찾기
    public int GetPartyIndex(int n)
    {
        return partys.FindIndex( i => i.dungeonNum == n);
    }
    
    public void RemoveTeam(int num)
    {

    }


    #region 용병 고용 관련

    //랜덤 용병생성
    public RandomKnight[] randomKnightList = new RandomKnight[3];

    public void CreateRandomKnight(int cnt)
    {
        for(int i =0 ; i < cnt; i++)
        {
            randomKnightList[i] = new RandomKnight(true);
        }
        DataController.Instance.AddRandomKnight(randomKnightList);
    }
    
    //용병고용. Check -> IsCheck()에서 호출됨.
    public void Employment(int i)
    {
        RandomKnight rn = randomKnightList[i];

        int[] arr = new int[4]{0 ,1, 2, 3};
        int hp = stateList[rn.job, rn.level- 1, 0];
        int num = knights.Count;
        knights.Add(new Knight(
            num, rn.name, rn.job, rn.level, 0,
            arr, 0, PlayerData.Instance.Day, rn.skinNum,
            true, hp, 0, arr
        ));
        
        //등록 및 전산처리
        DataController.Instance.AddKnight(knights[num]);
        DataController.Instance.UpdateRandomKnight(i);
        
        PlayerData.Instance.Gold -= knightPay[rn.level -1];
        randomKnightList[i].isEmploy = true;
        
    }

    #endregion
}
