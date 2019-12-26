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
    
    public void CreateEvent()
    {
        CreateDungeonEvents();
    }

    //수행해야할 이벤트가 남아있으면 true  없으면 false임
    public bool Check_RemainingEvent()
    {
        foreach(DungeonProgress dp in DungeonData.Instance.dungeon_Progress)
        {
            if(!(dp.eventType == 0 || dp.eventType == 8))
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
    public void CreateDungeonEvents()
    {
        foreach(DungeonProgress dp in DungeonData.Instance.dungeon_Progress)
        {
            if(dp.isParty == true)
            {
                if(dp.p.day >= 0)
                    CreateBattle(dp);
                else
                    dp.SetData(7);
            }
        }
    }

    void CreateBattle(DungeonProgress dp)
    {
        Dungeon d = dp.d;
        int[] cntList = new int[4]{5, 9, 13 ,17};
        // 1~4명 / 1~8명... 1~16명, 최대 페이즈 4
        
        int cnt = UnityEngine.Random.Range(UnityEngine.Random.Range(1, cntList[d.level-1] -1), cntList[d.level-1]); // (1 ~ cnt) ~ 4 사이의 값                    

        int[] arr = new int[cnt];   
        arr = d.GetMonster(cnt);

        //생성된 배틀데이타 저장.
        dp.Battle(arr);
    }

    //던전 클리어_ WorkListController _ 7 번에서 호출.
    public void Dungeon_Clear(int dpNum)
    {
        DungeonProgress dp = DungeonData.Instance.dungeon_Progress[dpNum];
        
        //보상등록
        Dungeon_Reward dr = DungeonData.Instance.dungeon_Rewards[dp.p.maxDayIndex];
        PlayerData.Instance.Gold += (int)((float)dp.Reward * (1.0f * dr.gold));
        dp.SearchP += (int)dr.search;

        //파티해제
        foreach(int k in dp.p.k)
            UnitData.Instance.knights[k].teaming = false;
        UnitData.Instance.partys.Remove(dp.p);

        //모드 변경
        dp.Dungeon_Reset();
        GameController.Instance.world.WorkList_Update();//화면 업뎃
    }
    #endregion
}
