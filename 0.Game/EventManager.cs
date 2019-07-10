using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Work
{
    public int type;
    public int who;
    public int[] what;

    /*************************************
    * type = 0 : 전투    int party.num    int[] monster
    *        1 : 
    * 
    * 
    * 
     ************************************/

     public Work(int _type, int _who, int[] _what)
     {
         type = _type;
         who = _who;
         what = _what;
     }
     public void What(int[] arr)
     {
        Array.Resize(ref what, arr.Length);
        what = arr;
     }
}

public class BattleLog
{
    Work w;
    Party p;
}

public class EventManager : MonoBehaviour
{
    public DungeonData dungeon;
    public UnitData unit;


    public List<Work> todayWork = new List<Work>();
    public List<Work> nextWork = new List<Work>();
    
    #region NextWork 추가 함수
    public void AddBattle(int who)
    {
        nextWork.Add(new Work(0, who, null)); 
    }

    #endregion

    #region World Work List관련
    public Text workText;
    public GameObject workPrefab;
    
    // * World - Bubble 버튼 text.
    public void SetText()
    {
        workText.text = string.Format("{0}", todayWork.Count);
    }
    
    //1. Bubble버튼 클릭 시 List 출력
    public void MakeWorkList(Transform list)
    {
        CodeBox.ClearList(list);

        foreach(Work w in todayWork)
        {
            GameObject obj = CodeBox.AddChildInParent(list, workPrefab);
            obj.GetComponent<WorkPrefab>().SetData(w);
            obj.GetComponent<WorkPrefab>().parent = this;
        }
    }

    public Sprite[] imgList;
    public Image left; //Work.type에 따른 이미지 변경.

    public static BattleLog log; //Battle에 저장될 work인자.
    public GameObject isCheck;
    //2. List에서 인자 클릭시 이벤트 실행.
    public void SelectWork(Work w)
    {
        //type에 알맞는 img로 WorkList - Left(obj)변경
        left.sprite = imgList[w.type];
        if(log.w != w) //첫클릭 인경우.
        {
            log.w = w;
        }
        else
        {

        }
    }
    

    // * Game - NextDay() 시,
    //   nextWork -> todayWork 로 변경

    public void ChangeWork()
    {
        foreach(Work w in nextWork)
        {
            switch(w.type)
            {
                //0. 몬스터와의 전투 시 이벤트.
                case 0: //0 : 전투    int party.num    int[] monster
                    Dungeon d = dungeon.dungeons[unit.partys[w.who].dungeonNum];
                    
                    // 1~4명 / 1~8명... 1~16명, 최대 페이즈 4
                    int cnt = d.level <= 5 ? 5 : d.level <= 10 ? 9 : d.level <= 15 ? 13 : 17 ; 
                    cnt = UnityEngine.Random.Range(UnityEngine.Random.Range(1, cnt), cnt); // (1 ~ cnt) ~ 4 사이의 값                    
                    
                    // * 랜덤 몬스터 카운트
                    //   추후 퍼센테이지 따서 확률로 몬스터 지정하기.
                    int mLen = d.monsters.Length; 
                    int[] arr = new int[cnt];
                    for(int i = 0; i< cnt; i++)
                    {
                        arr[i] = d.monsters[UnityEngine.Random.Range(0, mLen)];
                    }
                    //Work - What()로 인자 전달.
                    w.What(arr);
                    
                    //투데이에 추가해줌.
                    todayWork.Add(w);
                    break;
                default:
                    Debug.Log("오류");
                    break;
            }
        }
        //nextWork 초기화
        nextWork.Clear();
    }
    #endregion
}
