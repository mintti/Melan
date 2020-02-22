using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    public GameObject world;
    public Button _bt;
    private UnitData unit;

    public Button participateBt;
    private int cnt;
    public int Cnt{
        get{return cnt;}
        set{
            cnt = value;
            participateBt.interactable = cnt == 0 ? false : true;
        }
    }

    private DungeonData dungeon;
    public WorkListController workListController;
    //1-2. 던전 선택 후, 출전 클릭 시 : 변수 선언
    public GameObject selectKnighObj;
    public Transform selectKnightListTr;
    private Dungeon d; //col에서 호출된 d.
    
    //텍스쳐
    public KnightSkinPrefab[] SkinObjs = new KnightSkinPrefab[4];
    public Texture[] textures = new Texture[4];
    public List<int> txrArr = new List<int>();

    //초기 유닛데이타 컨넥팅
    private void Start()
    {
        unit = UnitData.Instance;
    }

    //던전 화면 세팅 / 던전Obj 생성 함수(매 씬 로드때마다 호출됨)
    public Transform[] dungeonPos = new Transform[8];
    public void CreateDungeonInWorld()
    {
        dungeon = DungeonData.Instance;
        for(int i =0 ;i < 8; i ++)
        {
            GameObject obj = CodeBox.AddChildInParent(dungeonPos[i], dungeon.dungeonObjArray[dungeon.dungeon_Progress[i].d.num]);
            obj.GetComponentInChildren<WorldDungeonPrefab>().SetData(dungeon.dungeon_Progress[i], i);
        }

        DungeonUpdate();
    }

    #region 1-3. 던전 선택 후, 출전 용병 List 생성 : 코드
    public GameObject knightList;
    public GameObject knightPrefab;

    public Transform selectView;
    public GameObject knightPrefabSelect;
    
    public void SelectDungeon(int n)
    {
        if(dungeon.CanGo(n) == false)
            return;
        
        d = dungeon.dungeon_Progress[n].d;

        workListController.SelectDungeon(n);
       
    }
    
    //출전가능한 기사 생성 및 창 true
    public SelectDayList selectDayList;
    public void MakeKnightList()
    {
        selectKnighObj.SetActive(true);

        CodeBox.ClearList(selectKnightListTr);

        foreach(Knight k in unit.knights)
        {
            if(k.teaming == true)
                continue;
            GameObject obj = CodeBox.AddChildInParent(selectKnightListTr, knightPrefab);
            
            obj.GetComponent<WorldKnightPrefab>().SetData(k);
        }

        //초기화.
        
        selectDayList.SetData(dungeon.GetDungeonProgress(d).SearchP == 100 ? true : false);

        CodeBox.ClearList(selectView);
        txrArr.Clear();
        Cnt = 0;
    }
    public int dayIndex;
    
    //파티 List & 오브젝트(Select View)에 요소 추가
    public void AddKnightInParty(Knight k)
    {
        if(Cnt == 4)
            return;
        WorldKnightSelect obj = CodeBox.AddChildInParent(selectView, knightPrefabSelect).GetComponent<WorldKnightSelect>();

        //사용하지 않는 Texture 찾아 출력
        int i;
        for(i = 0 ; i < 4 ; i ++)
        {
            if(txrArr.Contains(i) == false)
                break;
        }
        
        txrArr.Add(i);
        SkinObjs[i].SetData(k);
        obj.SetData(k.num, textures[i], i);
        
        Cnt++;
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
            WorldKnightSelect com = t.GetComponent<WorldKnightSelect>();
            if(k.num == com.kNum)
            {
                txrArr.Remove(com.txrNum);
                Cnt--;
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
            arr[i] = t.GetComponent<WorldKnightSelect>().kNum;
        }

        //배열의 마지막에 Dungeon정보 저장.
        arr[arr.Length - 1] = d.num;
        //UnitData - Partys 리스트에 추가
        DungeonData.Instance.dungeon_Progress[workListController.index].SendDungeon(unit.AddParty(arr, dayIndex));
        
        //작업 종료.
        selectKnighObj.SetActive(false);
        DungeonUpdate();

        WorkList_Update();
    }
    public void WorkList_Update()
    {
        workListController.SelectDungeon();
    }
    #endregion

    //게임 시작 시, 화면 초기화.
    public void Click()
    {
        _bt.onClick.Invoke();
    }

    //던전 화면 업데이트. (퀘스트가 있으면 !가뜨게한다.)
    //이벤트용버튼, 
    public void DungeonUpdate()
    {
        for(int i= 0 ; i < 8; i ++)
        {
            int type = DungeonData.Instance.dungeon_Progress[i].eventType;
            
            bool value = type == 0 || type == 8 || type == 2 ? false : true;
            dungeonPos[i].GetComponentInChildren<WorldDungeonPrefab>().SetEvent(value);
        }

    }

    //Choice Evnet. 화면닫기
    public void Choice_Event_Close()
    {
        workListController.choiceEvent.Close();
        workListController.gameObject.SetActive(false);
    }
    #region Texture Finder

    public void ResetTexture()
    {
        txrArr.Clear();
        Cnt = 0;
    }
    public Texture TextureFinder(Knight k)
    {
        int i;
        for(i = 0 ; i < 4 ; i ++)
        {
            if(txrArr.Contains(i) == false)
                break;
        }
        
        txrArr.Add(i);
        SkinObjs[i].SetData(k);
        
        Cnt++;

        return(textures[i]);
    }
    #endregion

}
