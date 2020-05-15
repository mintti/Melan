using System.Collections;
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

    private void Awake() {
        Create_Dungeon_Event = new Delegate[4];
        //Create_Dungeon_Event[0];
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
    private Delegate[] Create_Dungeon_Event;
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
                if(dp.p.dayIndex == 5) //보스일 경우 지정된 특별한 이벤트들 생성함.
                {
                    BattleEventDelegate[3]();
                }
                else if(dp.p.day >= 0) //보스가 아닌 일반..
                {
                    value = UnityEngine.Random.Range(0, dp.SearchP < 30 ? 3 : 4);
                    BattleEventDelegate[value]();
                }
                else//Day가 -1 이니 탐색완료.
                    dp.SetData(7);
            }
        }
    }

    private Delegate[] BattleEventDelegate;
    private void SetEventDelegate()
    {
        BattleEventDelegate = new Delegate[4]{
            Create_Event_None,
            Create_Event_Battle,
            Create_Event_Choice,
            Create_Event_Battle_Boss
        };
    }
    
    private void Create_Event_None()
    {
        dp.SetData(2);
    }
    //Battle 생성
    private void Create_Event_Battle()
    {
        dungeonEventMaker.GetRandomMonster(dp.d);
    }

    //Boss 이벤트 생성 : 지정된 값을 가져옴.
    private void Create_Event_Battle_Boss()
    {
        dungeonEventMaker.SetDungeonEvent(dp.d.num);
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

    public DungeonEventMaker dungeonEventMaker;
    public void Create_Event_Dungeon_0_Boss()
    {
        
    }
    #endregion

}