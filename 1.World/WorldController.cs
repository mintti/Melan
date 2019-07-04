using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    

    public GameObject world;
    public Button _bt;
    public UnitData unit;

    //1-1. 던전 선택 시, 가운데로 이동 : 변수 선언
    public RectTransform map;
    private Coroutine  coroutine; //코루틴 정지를 위해 미리 선언
    private float xVelocity = 2.0f; //속도, 상세 설명 해당 사용처에 적어둠.
    private float yVelocity = 2.0f;
    
    private bool corou_RN = false; //코루틴 충돌방지.

    //1-2. 던전 선택 후, 출전 클릭 시 : 변수 선언
    public GameObject selectKnighObj;
    public DungeonCol col;
    private Dungeon d; //col에서 호출된 d.

    //1-3.던전 선택 후, 출전 시, 용병 List관련. : 변수 선언
    public GameObject knightList;
    public GameObject knightPrefab;

    public Transform selectView;


    //출전 시 이벤트 
    public EventManager event_;



    //UI - World선택시 위치 0 0으로 초기화
    public void ResetPos()
    {
        map.anchoredPosition = new Vector2(0,0);
    }
    
    
    #region 1-1. 던전 선택 시, 가운데로 이동 : 코드
    //Dungeon 오브젝트 클릭 시, 해당 obj가 인자가 됨. (한정적이라 일일이 지정해주는 걸루함)
    public void ScreenMoveToDungeon(RectTransform tr)
    {
        Vector2 targetPos = new Vector2(tr.localPosition .x * -1, tr.localPosition.y * -1);
        if(corou_RN) //코루틴 충돌 방지
        {
            StopMoveCorou();
        }
        coroutine = StartCoroutine(move1_1(targetPos));
        corou_RN = true;
    }
    //코루틴 실행 상태에서 타 스크린 클릭시 코루틴 정지
    public void StopMoveCorou()
    {
        if(corou_RN) //코루틴 충돌 방지
        {
            StopCoroutine(coroutine);
            corou_RN = false;
        }
    }

    //해당 위치로 map을 이동해줌. float라 무한 루틴 걸릴 수도 있어서 오차 ±1해줌.
    public IEnumerator move1_1(Vector2 target)
    {
        corou_RN = true;
        while(true)
        {
            //map이 타겟에 도달시 코루틴 해제. (시간 대비로 x, y 값 계산해서 이동하는 거라 비례되니 x만 비교해도됨.)
            if(target.x < map.anchoredPosition.x +1 && target.x > map.anchoredPosition.x -1)
            {
                StopMoveCorou();
            }

            //SmoothDamp(현재 위치 값, 타겟 위치, 현재 속도 : 매번 호출함으로써 수정됨, 타겟에 도착하는 대략적인 시간)
            float newPosX = Mathf.SmoothDamp(map.anchoredPosition.x, target.x, ref xVelocity, 0.2f);
            float newPosY = Mathf.SmoothDamp(map.anchoredPosition.y, target.y, ref yVelocity, 0.2f);
            map.anchoredPosition = new Vector2(newPosX, newPosY);

           yield return new WaitForSeconds(0.01f);
        }
         
        corou_RN = false;
    }
    #endregion


    #region 1-2. 던전 선택 후, 출전 클릭 시 : 코드
    public void SelectDungeon()
    {
        d = col.d;
        if(d.isAdmit() && unit.GetPartyIndex(d.num) == -1)
        {
            selectKnighObj.SetActive(true);
        }
        
    }

    #endregion

    #region 1-3. 던전 선택 후, 출전 용병 List 생성 : 코드  
    public void MakeKnightList(Transform list)
    {
        CodeBox.ClearList(list);

        foreach(Knight k in unit.knights)
        {
            GameObject obj = CodeBox.AddChildInParent(list, knightPrefab);
            obj.GetComponent<WorldKnightPrefab>().SetData(k);
        }

        //용병 선택 창 초기화.
        CodeBox.ClearList(selectView);
    }

    //파티 List & 오브젝트(Select View)에 요소 추가
    public void AddKnightInParty(Knight k)
    {
        WorldKnightPrefab obj = CodeBox.AddChildInParent(selectView, knightPrefab).GetComponent<WorldKnightPrefab>();

        //셀렉트 뷰 내부에 있기 때문에 데이터 호출 후, 기능 제한.
        obj.KngihtInSelectView(k);
       
    }
    //파티 List & 오브젝트(Select View)에서 삭제
    public void RemoveKnightInParty(Knight k)
    {
        foreach(Transform t in knightList.transform)
        {
            WorldKnightPrefab obj = t.GetComponent<WorldKnightPrefab>();
            if(k.num == obj.k.num)
            {
                obj.IsType(0);
                break;
            }
                
        }
        foreach(Transform t in selectView)
        {
            Knight k_ = t.GetComponent<WorldKnightPrefab>().k;
            if(k.num == k_.num)
            {
                Destroy(t.gameObject);
                break;
            }
        }
    }
    #endregion

    #region 1-4. 출전! (xml추가 필)
    public void ParticipateInDungeon()
    {
        //size는 출전 창에서 선택 창에 존재하는 자식 오브젝트 수로 카운트.
        int size = selectView.childCount;
        int[] arr = new int[size + 1]; //[0~size -1 : 기사 num, size : 던전 num ]

        //셀렉뷰에 존재하는 기사의 num을 추출해 arr에 삽입
        for(int i = 0 ; i < size; i++)
        {
            Transform t = selectView.GetChild(i);
            Knight k = t.GetComponent<WorldKnightPrefab>().k;

            arr[i] = k.num;
        }

        //배열의 마지막에 Dungeon정보 저장.
        arr[arr.Length- 1] = d.num;
        
        //UnitData - Partys 리스트에 추가 / EvnetManger - AddBattle()해서 상세 데이터 입력. 
        unit.AddParty(arr);
        
        event_.AddBattle(unit.GetPartyIndex(d.num));
        //작업 종료.
        selectKnighObj.SetActive(false);
    }

    #endregion


    //게임 시작 시, 화면 초기화.
    public void Click()
    {
        _bt.onClick.Invoke();
    }




}
