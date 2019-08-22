using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class EventManager : MonoBehaviour
{
    private DungeonData dungeon;
    private UnitData unit;
    private EventData data;

    private void Start()
    {
        data = EventData.Instance;
        unit = DataController.Instance.unit;
        dungeon = DataController.Instance.dungeon;
        SetText();
    }

    #region NextWork 추가 함수
    //이벤트 생성 함수.
    public void NextDay()
    {
        //1. 파티 기반 전투 및 이벤트 생성

        //2. 업무 기반
    }
    #endregion

    #region World Work List관련 + 이벤트의 실행
    public Text workText;
    public GameObject workPrefab;
    
    // * World - Bubble 버튼 text.
    public void SetText()
    {
        workText.text = string.Format("{0}", EventData.Instance.todayWork.Count);
    }
    
    //1. Bubble버튼 클릭 시 List 출력
    public void MakeWorkList(Transform list)
    {
        CodeBox.ClearList(list);

        foreach(Work w in EventData.Instance.todayWork)
        {
            GameObject obj = CodeBox.AddChildInParent(list, workPrefab);
            obj.GetComponent<WorkPrefab>().SetData(w);
            obj.GetComponent<WorkPrefab>().parent = this;
        }
    }

    public Sprite[] imgList;
    public Image left; //Work.type에 따른 이미지 변경.
    
    public GameObject isCheck;
    private Transform tr_Work = null; //더블클릭인가? 
    //2. List에서 인자 클릭시 이벤트 실행.
    public void SelectWork(Transform tr)
    {
        //type에 알맞는 img로 WorkList - Left(obj)변경
        Work w = tr.GetComponent<WorkPrefab>().w;
        left.sprite = imgList[w.type];
        if(tr_Work != tr) //첫클릭 인경우.
        {
            tr_Work = tr;
        }
        else //더블클릭한 경우
        {
            isCheck.SetActive(true);
        }
    }

    public void SelectWork_Check()
    {
        
    }
    
    /* Game - NextDay() 시,
    TodayWork 업무 생성 함수
    */
    public void CreateWork()
    {
        //초기화
        EventData.Instance.Reset();

        //업무 생성
        CreatePartyEvent();
        ChangeWork();
    }

    // * Party 기반으로 이벤트 생성
    public void CreatePartyEvent()
    {
        foreach (Party p in unit.partys)
        {
            if(p.day != 0)
            {
                //확률 계산 후 배틀 이외 이벤트의 경우 아래에서 수행 후 탈출
                if(false)
                    return;
            }
            //N. Battle 생성
            Dungeon d = dungeon.dungeons[p.dungeonNum];
            
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
            //Battle 생성 및 인자 전달.
            Battle b = new Battle(p, arr);
            EventData.Instance.battles.Add(b);
            
            int index = EventData.Instance.battles.IndexOf(b);
            //투데이에 추가해줌.
            EventData.Instance.todayWork.Add(new Work(0, index));
        }
    }

    
    /* 
       190819_이 부분은 업무 관련 이벤트 변경..?      */
    public void ChangeWork()
    {
        foreach(Work w in EventData.Instance.nextWork)
        {
            switch(w.type)
            {
                default:
                    Debug.Log("오류");
                    break;
            }
        }
    }
    #endregion
}
