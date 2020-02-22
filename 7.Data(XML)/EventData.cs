﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EventData : MonoBehaviour
{
    private static EventData _instance;
    public static EventData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(EventData)) as EventData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active EventData object");
                }
            }
            return _instance;
        }
    }

    public void CreateEvent()
    {
        CreateDungeonEvents();
    }

    //수행해야할 이벤트가 남아있으면 true  없으면 false임
    private List<int> pass_Event_Type_List = new List<int>(){0, 2, 8};
    public bool Check_RemainingEvent()
    {
        foreach(DungeonProgress dp in DungeonData.Instance.dungeon_Progress)
        {
            if(!( pass_Event_Type_List.Contains(dp.eventType)))
                return true; 
        }
        return false;
    }

    #region 전투관련
    public DungeonProgress battle_dp;//이멘트 보내기 전투 데이타.

    public void SetBattleData(int index)
    {
        battle_dp = DungeonData.Instance.dungeon_Progress[index];
    }

    public void CompleteBattle()
    {
        //Battle 씬에서 수행.

        //보상 누적과
        //battles리스트, XmlData 제거
    }
    #endregion
    #region 던전 기반 이벤트 관리
    //GameController - ConnectData()
    //DungeonProgerss isParty값을 true 로 변경해줌.,(추가 출전을 할 수 없도록)  
    public void Connect_DungeonEvent_And_Party()
    {
        List<int> list = new List<int>();

        foreach(Party p in UnitData.Instance.partys)
        {
            for(int i = 0; i< 8;i ++)
            {
                if(DungeonData.Instance.dungeon_Progress[i].d.num == p.dungeonNum)
                    DungeonData.Instance.dungeon_Progress[i].Connect_Party(p);
            }   
            
        
        }
    }
    //생성 (GameController NextDay?)
    private DungeonProgress dp;
    public void CreateDungeonEvents()
    {
        int value;
        foreach(DungeonProgress dp_ in DungeonData.Instance.dungeon_Progress)
        {
            dp = dp_;
            if(dp.isParty == true)
            {
                if(dp.p.day >= 0)
                {
                    value = UnityEngine.Random.Range(0, dp.SearchP < 30 ? 3 : 4);
                    Create_Event(value);
                }
                else//Day가 -1 이니 탐색완료.
                    dp.SetData(7);
            }
        }
    }


    private void Create_Event(int value)
    {
        switch (value)
        {
            case 0 ://NON Event
                dp.SetData(2);
                break;
            case 1 : //Battle Event
                Create_Event_Battle();
                break;
            case 2 ://Choice Event
                Create_Event_Choice();
                break;
            case 3 ://Boss Event?
                break;
            case 4 :
                break;
            default:
                Debug.Log("예외발생");
                break;
        }
    }

    //Battle 생성
    void Create_Event_Battle()
    {
        Dungeon d = dp.d;
        //int[] cntList = new int[4]{5, 9, 13 ,17};
        // 1~4명 / 1~8명... 1~16명, 최대 페이즈 4
        int cnt = UnityEngine.Random.Range(UnityEngine.Random.Range(1, 4), 5);
        //int cnt = UnityEngine.Random.Range(UnityEngine.Random.Range(1, cntList[d.level-1] -1), cntList[d.level-1]); // (1 ~ cnt) ~ 4 사이의 값                    

        int[] arr = new int[cnt];   
        arr = GetMonster(d, cnt);

        //생성된 배틀데이타 저장.
        dp.Battle(arr);
    }
    //Monster생성
    private int[] GetMonster(Dungeon d, int cnt)
    {
        int[] array = new int[cnt];
        DungeonProgress dp = DungeonData.Instance.GetDungeonProgress(d.num);
        
        int level = dp.SearchP < 25 ? 0 : dp.SearchP < 50 ? 1 : dp.SearchP < 75 ? 2 : dp.SearchP < 100 ? 3 : 4;
        for(int i = 0 ; i < cnt ; i ++)
        {
            int m = 0;
            int randomNum = UnityEngine.Random.Range(1, 11);
            switch(level)
            {
                case 0 : // 0:1 = 7:3;
                    m = randomNum <= 7 ? 0 : 1 ;
                    break;
                case 1 : // 0:1:2 = 2:6:2 
                    m = randomNum <= 2 ? 0 : randomNum <= 8 ? 1 : 2;
                    break; 
                case 2 : // 1:2:3 = 5:4:1
                    m = randomNum <= 5 ? 1 : randomNum <= 9 ? 2 : 3;
                    break;
                case 3 : //1:2:3 = 3:5:2
                    m = randomNum <= 3 ? 1 : randomNum <= 8 ? 2 : 3;
                    break;
                case 4 : //2:3 = 7:3
                    m = randomNum <= 7 ? 2 : 3;
                    break;
                default : break;
            }
            array[i] = d.monsters[m];
        }
        
        return array; 
    }
    private int[] SetMonster_Boss(Dungeon d, int cnt)
    {
        int[] array = new int[5]{0,0,0,0,0};
        return array;
    }
    //Choice 생성
    private void Create_Event_Choice()
    {
        //test : 0_길선택지    1_행운의 샘물   2_보물상자
        int value = UnityEngine.Random.Range(0, 3);
        dp.Choice(value);
    }

    //던전 클리어_ WorkListController _ 7 번에서 호출.
    public void Dungeon_Clear(int dpNum)
    {
        DungeonProgress dp = DungeonData.Instance.dungeon_Progress[dpNum];
        
        //보상등록
        Dungeon_Reward dr = DungeonData.Instance.dungeon_Rewards[dp.p.dayIndex];
        
        PlayerData.Instance.Gold += dp.Get_Dungeon_Reward_Gold();    
        dp.SearchP += (int)dr.search;

        //파티해제
        foreach(int k in dp.p.k)
            UnitData.Instance.knights[k].teaming = false;
        UnitData.Instance.partys.Remove(dp.p);

        //모드 변경
        dp.Dungeon_Reset();
        GameController.Instance.world.WorkList_Update();//화면 업뎃

        GameController.Instance.EventCheck();
    }
    #endregion

}
