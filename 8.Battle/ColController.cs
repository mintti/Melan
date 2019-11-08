using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColController : MonoBehaviour
{
    private static ColController _instance;
    public static ColController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(ColController)) as ColController;

                if(_instance == null)
                {
                    Debug.LogError("There's no active ColController object");
                }
            }
            return _instance;
        }
    }

    /*
        1. SkillForm에서 Skill을 입력 받을 경우 icon을 알맞게 수정해줍니다.
        2. 수많은 COL을 전투 상황에 맞게 배치해주는 함수입니다.
        3. 입력 받은 Form을 생성해준다.
    */
    public GameObject BattleUI;

    public GameObject knightArea_ALL;
    public GameObject[] knightArea_4 = new GameObject[4];

    public GameObject monsterArea_ALL;
    public GameObject monsterArea_BOSS;
    public GameObject[] monsterArea_4 = new GameObject[4];//미사용 예정..
    public GameObject[] monsterArea_HALF_4 = new GameObject[4];
    public GameObject[] monsterArea_HALF_2 = new GameObject[2];
    
    public GameObject NoneArea;
    public Sprite testSkillspr;

    List<GameObject> objList = new List<GameObject>();
    Skill skill;

    //Form 관련 
    public GameObject[] formList = new GameObject[10];
    public GameObject[] formObj;

    //Battle진행2
    private int behavior;
    public int Behavior{get{return behavior;} 
        set{behavior = value; if(behavior == 0) TurnEnd();}}
    private int who;
    
    #region FORM SETTING
    public void SetForm()
    {
        Battle b = EventData.Instance.Bdata;
        formObj = new GameObject[b.p.k.Length];

        int index = 0;
        foreach(int kNum in b.p.k)
        {
            GameObject form = formList[UnitData.Instance.knights[kNum].job];
            formObj[index] = CodeBox.AddChildInParent(BattleUI.transform, form);

            formObj[index].GetComponent<Form>().Test();    
            formObj[index++].SetActive(false);
        }
        Debug.Log("SetForm _ Complete");
    }

    public void TurnStart(int _who)
    {
        who = _who;
        
        formObj[who].SetActive(true);
    }
    private void TurnEnd()
    {
        formObj[who].SetActive(false);
        BattleController.Instance.NextTurn();
    }
    
    #endregion
    #region COL SETTING 
    //각 Skill - PushComplete()에서 호출됨. 
    public void SetData(Skill _skill)
    {   
        skill  = _skill; //인자 저장.

        BattleUI.SetActive(false);
        ChangeColForm(true);
    }

    //입력 받은 SKill을 분석하여 알맞게 COL폼을 변경해준다.
    public void ChangeColForm(bool value)
    {
        if(value)
        {
            objList.Clear();

            switch(skill.target)
            {
                case Target.NONE :
                    SetForm(0);
                    break;
                case Target.MINE :
                    
                    break;
                case Target.WE :
                    SetForm(1);
                    break;
                case Target.THAT :

                    break;
                case Target.THEY :
                    SetForm(2);
                    break;
                default : 
                    Debug.Log("예외값 입력됨");
                    break;
            }
        }
        foreach(GameObject obj in objList)
        {
            obj.SetActive(value);
        }
    }

    
    private void SetForm(int target)
    {
        //target. 0: NONE스킬. 1: KNIGHT. 2. MONSTER
        switch(target)
        {
            case 0:
                objList.Add(NoneArea);
                break;  
            case 1:
                objList.Add(knightArea_ALL);
                break;
            case 2:
                objList.Add(monsterArea_ALL);
                break;
            default : 
                Debug.Log("예외값이 입력됨.");
                break;
        }
    }
    //Part로 나눠진..
    private void SetForm(int target, int index)
    {   //0: KNIGHT 2. MONSTER_하프_하프 3. MONSTER_하프_쿼터 4. BOSS
        switch(target)
        {
            case 1 :
                objList.Add(knightArea_4[index]);
                break;
            case 2 :
                objList.Add(monsterArea_HALF_2[index]);
                break;
            case 3 :
                objList.Add(monsterArea_HALF_4[index]);
                break;
            case 4:
                objList.Add(monsterArea_BOSS);
                break;
            default : 
                Debug.Log("예외값이 입력됨.");
                break;

        }
    }
    #endregion

    #region SkillDrag 관련
    public Transform invisibleSkill;
    
    public static void SwapKSkills(Transform sour, Transform dest)
    {
        Transform sourParent = sour.parent;
        Transform destParent = dest.parent;

        int sourIndex = sour.GetSiblingIndex();
        int destIndex = dest.GetSiblingIndex();

        sour.SetParent(destParent);
        sour.SetSiblingIndex(destIndex);

        dest.SetParent(sourParent);
        dest.SetSiblingIndex(sourIndex);
    }

    void SwapSkillHierarchy(Transform sour, Transform dest)
    {
        SwapKSkills(sour, dest);
    }

    public void BeginDrag(Transform skill)
    {

        SwapSkillHierarchy(invisibleSkill, skill);
    }
    
    //누군가에게 col하지 않은채로 끝났다.
    public void EndDrag(Transform skill)
    {
        ChangeColForm(false);
        BattleUI.SetActive(true);
        SwapSkillHierarchy(invisibleSkill, skill);
    }
    #endregion
}
