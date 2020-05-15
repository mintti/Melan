using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEventMaker : MonoBehaviour
{
    private Delegate[][] DungeonEvent_Del;//전체 이벤트함수?
    private List<int[]> EventIndexList = new List<int[]>();//각 던전별 Evnet Num;
    private void Awake()
    {
        DungeonEvent_Del = new Delegate[1][];


        //Evnet List // 순서는 0~16던전
        EventIndexList.Add(new int[5]{0,1,2,3,4});
    }

    private void SetDelegate()
    {
        DungeonEvent_Del[0] = new Delegate[5]{D00_01, D00_02, D00_03, D00_04, D00_05};

    }
    
    private DungeonProgress dp;
    public void SetDungeonEvent(int num)//dp 의 index;
    {
        dp  = DungeonData.Instance.dungeon_Progress[num];
        int dungeonNum = dp.d.num;

        DungeonEvent_Del[dungeonNum][dp.p.Day]();
    }

    private void SetBattle(int[] arr)
    {
        dp.Battle(arr);
    }
    private void SetBoss(int[] arr)
    {
        dp.Battle_Boss(arr);
    }
    private void SetChoice(int num)
    {
        dp.Choice(num);
    }
    private void SetNoneEvent()
    {
        dp.SetData(2);
    }

    //랜덤 몬스터 로직..
    public void GetRandomMonster(Dungeon d)
    {
        dp = DungeonData.Instance.dungeon_Progress[d.num];
        
        int cnt = UnityEngine.Random.Range(UnityEngine.Random.Range(1, 3), 5); 
        int[] array = new int[cnt];
        
        
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
        
        SetBattle(array);
    }

    #region 00슬라임
    private void D00_01()
    {
        SetBattle(new int[3]{0, 0, 1});
    }
    private void D00_02()
    {
        SetNoneEvent();
    }
    private void D00_03()
    {
        SetNoneEvent();
    }
    private void D00_04()
    {
        SetNoneEvent();
    }
    private void D00_05()
    {
        SetBoss(new int[5]{0, 2, 3, 3, 4});
    }
    #endregion

    
}
