using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkListController : MonoBehaviour
{
    public WorldController world;

    public GameObject[] workTypeObj = new GameObject[9];
    public GameObject partyInfo_Button;
    static int type;

    public Text dungeon_Name_Text;
    public Text dungeon_Level_Text;
    public Text dungoen_SearchPer_Text;
    public Text dungeon_SaturationPer_Text;
    public Text button_Ment_Text;
    public Text info_Ment_Text;

    public Button button;
    public int index;

    //#8 귀환가능 화면
    public Text party_Gold_Text;
    public Text party_Turn_Text;
    public void SelectDungeon()
    { SelectDungeon(index);}
    public void SelectDungeon(int _index)
    {
        //기본 던전 정보
        index = _index;

        gameObject.SetActive(true);

        DungeonProgress dp = DungeonData.Instance.dungeon_Progress [index];

        dungeon_Name_Text.text = dp.d.name;
        dungeon_Level_Text.text = index <2 ? "LV.1" : index < 5 ? "LV.2" : index < 7 ? "LV.3" : "LV.4";
        dungoen_SearchPer_Text.text = string.Format("{0}%", dp.SearchP);
        dungeon_SaturationPer_Text.text = string.Format("{0}%", dp.Saturation);
        
        type = DungeonData.Instance.dungeon_Progress[index].eventType;
        
        //화면 세팅
        /* 0 : none(출전가능한)
           1 : 전투
           2 : 자연지형에 의한 선택지
           3 : 동료간 대화에 의한 선택지
           4 : 기록의 발견(SearchPer 첫달성시)
           
           7 : 일정대로 진행 후 귀환함.
           8 : 0의 제외한 이벤트 후 귀환 요청.
        */
        for(int i = 0 ;i < 9 ;i ++) workTypeObj[i].SetActive(false);
        
        workTypeObj[type].SetActive(true);

        switch (type)
        {
            case 1 :
                workTypeObj[type].GetComponentInChildren<Text>().text = string.Format("X {0}", dp.m.Length);
                break;
            case 7 :
                
                break;
            case 8 :
                party_Gold_Text.text = string.Format("{0}G", dp.Reward);
                party_Turn_Text.text = string.Format("{0}T", dp.p.day);
                break;
            default:
                break;
        }
        workTypeObj[type].SetActive(true);
        //멘트
        button_Ment_Text.text = TextData.Instance.workList_Button_Ment_Text[type];
        info_Ment_Text.text = TextData.Instance.workList_Info_Ment_Text[type];
        // button.Interactable = n ==
        
        CodeBox.ClearList(partyInfo_ListTr);
        if(type!=0 && type !=7) PartyInfoViewer();
    }
    


    public void Click()
    {
        switch (type)
        {
            case 0 :
                GameController.Instance.world.MakeKnightList();
                break;
            case 1 : 
                EventData.Instance.SetBattleData(index);
                GameController.Instance.LoadScene(2);
                break;
            case 7 :
                EventData.Instance.Dungeon_Clear(index);
                break;
            default:
                break;
        }
    }

    private void ReturnParty()
    {//파티가 존재 -> 이벤트 수행 완료 -> 귀환가능상태(8)

    }

    private void SetList()
    {

    }
    public void Close()
    {
        gameObject.SetActive(false);
        //workTypeObj[type].SetActive(false);
    }

    #region 파티 리스트

    public Transform partyInfo_ListTr;
    public GameObject partyInfo_knight;

    private void PartyInfoViewer(bool value)
    {

    }
    public void PartyInfoViewer()
    {
        Party p = DungeonData.Instance.dungeon_Progress[index].p;

        CodeBox.ClearList(partyInfo_ListTr);

        world.ResetTexture();//<< -- 텍스쳐임
        foreach(int kNum in p.k)
        {
            GameObject obj = CodeBox.AddChildInParent(partyInfo_ListTr, partyInfo_knight);

            Knight k = UnitData.Instance.knights[kNum];
            obj.GetComponent<KnightInfo_Simple>().SetData(k);
            
            obj.GetComponent<KnightInfo_Simple>().SetCharacter(world.TextureFinder(k));//<<-- 텍스쳐
        }
    }

    #endregion
}