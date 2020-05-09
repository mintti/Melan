using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEventMaker : MonoBehaviour
{
    private Delegate[] DungeonEvent;//전체 이벤트함수?
    private List<int[]> EventIndexList = new List<int[]>();//각 던전별 Evnet Num;
    private void Awake()
    {
        DungeonEvent = new Delegate[5];


        //Evnet List // 순서는 0~16던전
        EventIndexList.Add(new int[5]{0,1,2,3,4});
    }

    private void SetDelegate()
    {

    }
    
    private DungeonProgress dp;
    public void SetDungeonEvent(int num)//dp 의 index;
    {
        dp  = DungeonData.Instance.dungeon_Progress[num];
        int dungeonNum = dp.d.num;

        //최종보스의 경우
        if(dp.p.dayIndex == 5 && dp.p.Day == 0)
        {
            dp.Battle_Boss(new int[4]{2,3,3,4});
            return;
        }
        //아닐경우, 이벤트 List설정
        int[] array = EventIndexList[dungeonNum];

    }
    
    #region 랜덤지정자
  
    #endregion
}
