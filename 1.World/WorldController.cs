using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    

    public GameObject world;
    public Button _bt;
    private UnitData unit;

    //1-2. 던전 선택 후, 출전 클릭 시 : 변수 선언
    public GameObject selectKnighObj;
    private Dungeon d; //col에서 호출된 d.

    //1-3.던전 선택 후, 출전 시, 용병 List관련. : 변수 선언
    public GameObject knightList;
    public GameObject knightPrefab;

    public Transform selectView;


    //출전 시 이벤트 
    public EventManager event_;

    //초기 유닛데이타 컨넥팅
    private void Start()
    {
        unit = DataController.Instance.unit;
    }


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
        arr[arr.Length - 1] = d.num;
        //UnitData - Partys 리스트에 추가
        unit.AddParty(arr);
        
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
