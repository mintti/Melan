﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dungeon
{
    public int num;
    public string name{get;set;}
    public int level{get;set;}

    public int[] monsters = new int[5];
    public int boss;

    public Dungeon( string _name, int _level)
    {
        name = _name;
        level = _level;
    }

    public void SetData(int _num, string _name, int _level)
    {
        num = _num;
        name = _name;
        level = _level;
        
        int[] arr = new int[5];
        for(int i = 0 ;i <5; i++)
            arr[i] = 5 * num + i;
        
        SetMonster(arr);
    }
    
    private void SetMonster(int[] arr)
    {
        monsters = arr;
    }
}

public class DungeonProgress
{
    public Dungeon d;
    
    #region 개략적인 정보

    public bool isClear; //보스를 잡았는가~
    private double saturation;//포화도
    public double Saturation
    {
        get{return saturation;}
        set{saturation = value; if(saturation > 100) saturation = 100f;}
    }
    private double searchP;//진행도 (탐색 90/boss 10) : 50 부터 포화도 카운트
    public double SearchP
    {
        get{return searchP;}
        set{searchP = value; if(searchP > 100) searchP = 100f;}
    }
    private int reward;
    public int Reward{get{return reward;} set{reward = (int)value;}}
    public int experPoint;

    public DungeonProgress(){}
    public DungeonProgress(Dungeon _d)
    {//첫 데이터.
        d = _d;
        Reset();
    }
    public DungeonProgress(Dungeon _d, bool _isClear, double _saturation, double _searchP){
        d = _d;
        isClear = _isClear;
        Saturation = _saturation;
        SearchP = _searchP;
    }
    public void Reset()
    {
        isClear = false;
        Saturation = 50;
        searchP = 0;
        Reward = 0;

        Dungeon_Reset();
    }   

    #endregion

    #region DungeonEvent
    public int eventType;//WorkList참조.
    public int choice_Event_Type;
    public Party p;//컨넥으로 등록됨.

    //출전한 여부
    public bool isParty; //eventType이 0이 아닌경우 해당됨.
    public void Connect_Party(Party _p)
    {
        p = _p;
        isParty = true;
    }

    //type에 따른 선택
    public int[] m;
    
    public void SetData(int n)
    {
        eventType = n;
    }
    //해당 던전에 보낼떄(1일차세팅)
    public void SendDungeon(Party _p)
    {
        Reward= 0;
        eventType = 8;
        p = _p;
        isParty = true;
    }
    public void Battle(int[] arr)
    {
        eventType = 1;
        m = arr;
    }
    public void Choice(int n)
    {
        eventType = 3;
        choice_Event_Type = n;
    }
    public void Battle_Boss(int[] arr)
    {
        eventType = 4;
        m = arr;
    }

    //ClearReset. WorldCon - Dungeon_Clear()
    public void Dungeon_Reset()
    {
        eventType = 0;
        isParty = false;
        p = null;
    }
    #endregion

    //Reward
    public int Get_Dungeon_Reward_SearchPer()
    {
        return (int)DungeonData.Instance.dungeon_Rewards[p.dayIndex].search;
    }

    public int Get_Dungeon_Reward_Gold()
    {
        double gold = Reward * (1 + 0.01 * DungeonData.Instance.dungeon_Rewards[p.dayIndex].gold); 

        return (int)gold;
    }
}

public class Dungeon_Reward
{
    public float search;
    public float gold;

    public Dungeon_Reward(float s, float g)
    {
        search = s;
        gold = g;
    }
}
public class DungeonData : MonoBehaviour
{
    private static DungeonData _instance = null;

    public static DungeonData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(DungeonData)) as DungeonData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active DungeonData object");
                }
            }
            return _instance;
        }
    }

    public GameObject[] dungeonObjArray = new GameObject[16];

    public Dungeon[] dungeons = new Dungeon[16];
    public DungeonProgress[] dungeon_Progress = new DungeonProgress[8];
    //보상
    public Dungeon_Reward[] dungeon_Rewards = new Dungeon_Reward[6]{new Dungeon_Reward(5, 0), new Dungeon_Reward(10, 5), new Dungeon_Reward(15, 8),
                                            new Dungeon_Reward(20, 15), new Dungeon_Reward(30, 25),  new Dungeon_Reward(0, 0)};
    //데이
    public int[] day_Array = new int[6]{1, 2, 3, 4, 5, 5};//4, 8, 12, 16, 20

    public void SetData()
    {
        /*   level 1  */
        dungeons[0].SetData(0, "슬라임 군락", 1);
        dungeons[1].SetData(1, "음험한 산지", 1);
        dungeons[2].SetData(2, "맑은 오아시스", 1);
        dungeons[3].SetData(3, "의문의 땅굴", 1);

        /*   level 2  */
        dungeons[4].SetData(4, "비명의 산", 2);
        dungeons[5].SetData(5, "깊고 어두운 호수", 2);
        dungeons[6].SetData(6, "꽃피는 화원", 2);
        dungeons[7].SetData(7, "환각의 숲", 2);
        dungeons[8].SetData(8, "키큰 오두막", 2);
        dungeons[9].SetData(9, "의문의 탑", 2);
        
        /*   level 3   */
        dungeons[10].SetData(10, "황폐한 평원", 3);
        dungeons[11].SetData(11, "크로드네 령", 3);
        dungeons[12].SetData(12, "테레제바 국", 3);
        dungeons[13].SetData(13, "X의 골짜기", 3);

        /*   level 4   */
        dungeons[14].SetData(14, "마왕성", 4);
        dungeons[15].SetData(15, "이형의 땅", 4);

        day_Array = new int[6]{1, 2, 3, 4, 5, 5};
    }

    //해당하는 던전NUM의 던전진행의 인덱스를 출력
    public DungeonProgress GetDungeonProgress(Dungeon d)
    {
        for(int i = 0 ;i < dungeon_Progress.Length; i++)
        {
            if(dungeon_Progress[i].d  == d)
                return dungeon_Progress[i];
        }
        return null;
    }
    public int GetDungeonProgressIndex(DungeonProgress dp)
    {
        for(int i = 0 ;i < dungeon_Progress.Length; i++)
            if(dp == dungeon_Progress[i])
                return i;
        return 0;
    }
    public DungeonProgress GetDungeonProgress(int dNum)
    {
        return GetDungeonProgress(dungeons[dNum]);
    }

    //DataController에서 호출됨.
    //첫 시작시 던전 생성.
    public void DungeonMake()
    {
        int[] array = new int[8];
        /*          lv       dungeonArr[]
        0~3   : 1          2  0  -
        4~9   : 2          5  3  1
        10~13 : 3          7  6  4
        14~15 : 4
        */
        List<int> list = new List<int>(){ 0,1, 2,3, 4, 5,6, 7, 8,9,10, 11, 12, 13, 14,15};
        int[,] rangeList = new int[4, 2]{{0, 4}, {2, 8}, {5, 9}, {7, 9}};
        int[] con = new int[4]{2, 3, 2, 1};

        int index = 0;
        for(int i = 0 ; i < 4; i++)
        {
            for(int j = 0 ; j < con[i]; j ++)
            {
                array[index] = list[Random.Range(rangeList[i, 0], rangeList[i, 1]- j)];
                list.Remove(array[index++]);
            }
        }
        ConnectDungeon(array);
    }

    private void ConnectDungeon(int[] array)
    {
        for(int i = 0 ; i < 8; i++)
        {
            Dungeon d = dungeons[array[i]];
            dungeon_Progress[i] = new DungeonProgress(d);
        }
    }

    public bool CanGo(int n)
    {
        switch(n)
        {
            case 0 :
                return true;
            case 1 :
                return true;
            case 2 :
                return CanGo2(0);
            case 3 :
                return CanGo2(0)|CanGo2(1);
            case 4 :
                return CanGo2(1);
            case 5 :
                return CanGo2(2)|CanGo2(3);
            case 6 :
                return CanGo2(3)|CanGo2(4);
            case 7 :
                return CanGo2(5)|CanGo2(6);
            default :
                return false;
        }
    }
    public bool CanGo2(int n)
    {
        if(dungeon_Progress[n].SearchP == 100)
            return true;
        else return false;
    }
    
}
