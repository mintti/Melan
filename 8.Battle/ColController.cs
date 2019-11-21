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

    private void Start() {
        ResetForm();
    }
    /*
        1. SkillForm에서 Skill을 입력 받을 경우 icon을 알맞게 수정해줍니다. ==> SKill에서 구현됨.
        2. 수많은 COL을 전투 상황에 맞게 배치해주는 함수입니다.
        3. 입력 받은 Form을 생성해준다.
        4. Col 된 스킬을 저장하고 
    */
    public GameObject BattleUI;
    public Sprite testSkillspr;

    Skill skill;

    //Form 관련 
    public GameObject[] formList = new GameObject[10];
    public GameObject[] formObj;

    //Battle진행2
    private int behavior;
    public int Behavior{get{return behavior;} 
        set{behavior = value; if(behavior == 0) TurnEnd();}}
    private int who;
    
    #region SKILL FORM SETTING
    
    public void SetForm()
    {
        Battle b = EventData.Instance.Bdata;
        formObj = new GameObject[b.p.k.Length];

        int index = 0;
        foreach(int kNum in b.p.k)
        {
            GameObject form = formList[UnitData.Instance.knights[kNum].job];
            formObj[index] = CodeBox.AddChildInParent(BattleUI.transform, form);
  
            formObj[index++].SetActive(false);
        }
        Debug.Log("SetForm _ Complete");
    }

    public void TurnStart(int _who)
    {
        who = _who;
        
        formObj[who].SetActive(true);
        formObj[who].GetComponent<Form>().MyTurn();
    }
    private void TurnEnd()
    {
        formObj[who].SetActive(false);
        BattleController.Instance.NextTurn();
    }
    
    #endregion
    #region COL SETTING
    public GameObject monsterColForm;
    public GameObject[] monstersObj = new GameObject[4];
    public GameObject monsterAllObj;
    public GameObject bossObj;
    public GameObject knightsObj;

    //각 Skill - PushComplete()에서 호출됨. 
    public void SetData(Skill _skill)
    {   
        skill  = _skill; //인자 저장.

        BattleUI.SetActive(false);
        ChangeColForm();
    }

    //입력 받은 SKill을 분석하여 알맞게 COL폼을 변경해준다.
    public void ChangeColForm()
    {
        ResetForm();
        switch(skill.target)
        {
            case Target.NONE :
                SetColForm_KnightAll();
                break;
            case Target.MINE :
                    
                break;
            case Target.WE :
                SetColForm_KnightAll();
                break;
            case Target.THAT :
                SetColForm_Monster(GetMonsterIndex());
                break;
            case Target.THEY :
                SetColForm_MonsterAll(GetMonsterIndex());
                break;
            default : 
                Debug.Log("예외값 입력됨");
                break;
        }
    }
        

    bool[] GetMonsterIndex()
    {
        BattleController battle = BattleController.Instance;
        bool[] array = new bool[5];
        
        for(int i  = 0; i < 4; i++)
            array[i] = battle.monsterTarget.Contains(i + battle.knightCount);    
        

        array[4] = false;
        return array;
    }

    public void SetColForm_KnightAll()
    {
        knightsObj.SetActive(true);
        knightsObj.GetComponent<TargetArea>().SetData();
    }
    public void SetColForm_MonsterAll(bool[] value)
    {
        monsterColForm.SetActive(true);
        monsterAllObj.SetActive(true);
        monsterAllObj.GetComponent<TargetArea>().SetData();
        for(int i = 0 ;i < 4 ; i ++)
        {
            monstersObj[i].GetComponent<TargetArea>().SetData(true);
            monstersObj[i].SetActive(value[i]);
        }
        bossObj.GetComponent<TargetArea>().SetData(true);
        bossObj.SetActive(value[4]);
    }
    public void SetColForm_Monster(bool[] value)
    {
        monsterColForm.SetActive(true);
        for(int i = 0 ;i < 4 ; i ++)
        {
            monstersObj[i].GetComponent<TargetArea>().SetData();
            monstersObj[i].SetActive(value[i]);
        }
        bossObj.SetActive(value[4]);
    }

    private void ResetForm()
    {
        for(int i = 0 ;i < 4 ; i ++) monstersObj[i].SetActive(false);
        bossObj.SetActive(false);
        monsterAllObj.SetActive(false);
        knightsObj.SetActive(false);
        monsterColForm.SetActive(false);
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
        ResetForm();
        BattleUI.SetActive(true);
        SwapSkillHierarchy(invisibleSkill, skill);
    }
    #endregion
}
