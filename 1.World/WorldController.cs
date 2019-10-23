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

    //1-2. 던전 선택 후, 출전 클릭 시 : 변수 선언
    public GameObject selectKnighObj;
    private Dungeon d; //col에서 호출된 d.

    //1-3.던전 선택 후, 출전 시, 용병 List관련. : 변수 선언
    public GameObject knightList;
    public GameObject knightPrefab;

    public Transform selectView;
    public GameObject knightPrefabSelect;
    //텍스쳐
    public KnightSkinPrefab[] SkinObjs = new KnightSkinPrefab[4];
    public Texture[] textures = new Texture[4];
    public List<int> txrArr = new List<int>();

    //출전 시 이벤트 
    public EventManager event_;

    //초기 유닛데이타 컨넥팅
    private void Start()
    {
        unit = DataController.Instance.unit;
    }


    #region 1-3. 던전 선택 후, 출전 용병 List 생성 : 코드  
    public void WhatDungeon(int n)
    {
        d = DungeonData.Instance.dungeon_Progress[n].d;
    }
    public void MakeKnightList(Transform list)
    {
        CodeBox.ClearList(list);

        foreach(Knight k in unit.knights)
        {
            if(k.teaming == true)
                continue;
            GameObject obj = CodeBox.AddChildInParent(list, knightPrefab);
            
            obj.GetComponent<WorldKnightPrefab>().SetData(k);
        }

        //초기화.
        CodeBox.ClearList(selectView);
        txrArr.Clear();
        Cnt = 0;
    }

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
        SkinObjs[i].SetData(k.skin);
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
