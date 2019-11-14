﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dungeon
{
    public int num;
    public string name{get;set;}
    public int level{get;set;}
    public int day{get;set;}

    public int[] commonMonster;
    public int eliteMonster;
    public int boss;

    public Dungeon(string _name, int _level, int _boss)
    {
        name = _name;
        level = _level;
        boss = _boss;

        day = 3 * level; 
    }

    public void SetData(string _name, int _level, int _boss)
    {
        name = _name;
        level = _level;
        boss = _boss;

        day = 3 * level; 
        SetMonster();
    }
    
    public void SetMonster()
    {
        commonMonster = new int[3]{0, 0, 0};
        eliteMonster =0 ;
        boss = 0;
    }
    public int[] GetMonster(int size)
    {
        int[] array = new int[size];
        int m = 0;
        
        DungeonProgress dp = DungeonData.Instance.GetDungoenProgress(num);
        //M은 몬스터로.. dp에 의거하여 해당하는 몬스터정보를 Dungeon에서 가져옴.
        
        int difficulty = dp.GetDifficulty();
        int commonPer = difficulty == 0 ? 10 : 8 ;
        
        if(difficulty == 2)
        {
            commonPer = 9;
            array[size-1] = boss;
            size --;
        }

        for(int i = 0; i< size; i++)
        {
            array[i] = Random.Range(0, 10) < commonPer ? commonMonster[Random.Range(0, commonMonster.Length)] : eliteMonster;
        }
        
        return array; 
    }    
}

public class DungeonProgress
{
    public Dungeon d;
    
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

    public DungeonProgress(Dungeon _d)
    {
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
    }

    public int GetDifficulty()
    {
        return searchP < 50 ? 0 : searchP < 90 ? 1 : 2;
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

    public Dungeon[] dungeons = new Dungeon[16];
    public DungeonProgress[] dungeon_Progress = new DungeonProgress[8];

    public void SetData()
    {
        /*   level 1  */
        dungeons[0].SetData("슬라임 군락", 1 , 0);
        dungeons[1].SetData("음험한 산지", 1, 1);
        dungeons[2].SetData("맑은 오아시스", 1, 2);
        dungeons[3].SetData("의문의 땅굴", 1, 3);

        /*   level 2  */
        dungeons[4].SetData("비명의 산", 2, 4);
        dungeons[5].SetData("깊고 어두운 호수", 2, 5);
        dungeons[6].SetData("꽃피는 화원", 2, 6);
        dungeons[7].SetData("환각의 숲", 2, 7);
        dungeons[8].SetData("키큰 오두막", 2, 8);
        dungeons[9].SetData("의문의 탑", 2,  9);
        
        /*   level 3   */
        dungeons[10].SetData("황폐한 평원", 3, 10);
        dungeons[11].SetData("크로드네 령", 3, 11);
        dungeons[12].SetData("테레제바 국", 3, 12);
        dungeons[13].SetData("X의 골짜기", 3,13);

        /*   level 4   */
        dungeons[14].SetData("마왕성", 4, 0);
        dungeons[15].SetData("이형의 땅", 4, 0);
    }

    //해당하는 던전NUM의 던전진행의 인덱스를 출력
    public DungeonProgress GetDungoenProgress(Dungeon d)
    {
        for(int i = 0 ;i < dungeon_Progress.Length; i++)
        {
            if(dungeon_Progress[i].d  == d)
                return dungeon_Progress[i];
        }
        return null;
    }
    public DungeonProgress GetDungoenProgress(int dNum)
    {
        return GetDungoenProgress(dungeons[dNum]);
    }

    //DataController에서 호출됨.
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

        ConnectDungoen(array);
        DataController.Instance.SaveDungeon(array);
    }

    private void ConnectDungoen(int[] array)
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
