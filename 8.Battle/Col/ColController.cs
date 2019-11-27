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

    BattleController battle;
    private void Awake()
    {
        battle = BattleController.Instance;
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
    public int who{get;set;}
    
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

            formObj[index].GetComponent<Form>().SetData(battle.kps[kNum]);
            formObj[index++].SetActive(false);
        }
    }

    public void TurnStart(int _who)
    {
        who = _who;
        
        formObj[who].SetActive(true);
        formObj[who].GetComponent<Form>().MyTurn();
    }
    public void TurnEnd()
    {
        formObj[who].GetComponent<Form>().TurnEnd();
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
    public GameObject knightObj;

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
                SetColForm_Knight();
                break;
            case Target.MINE :
                SetColForm_Knights();
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
        bool[] array = new bool[5];
        
        for(int i  = 0; i < 4; i++)
            array[i] = battle.monsterTarget.Contains(i + battle.knightCount);    
        

        array[4] = false;
        return array;
    }

    private void SetColForm_Knight()
    {
        knightObj.SetActive(true);
        battle.kps[who].transform.GetComponent<KnightCol>().SetData(false);
        battle.kps[who].TargetOff();
    }
    private void SetColForm_Knights()
    {
        for(int i = 0 ;i <4; i++)
        {
            if(battle.kps[i].isKnight)
                battle.kps[i].transform.GetComponent<KnightCol>().SetData(true);
        }
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
            monstersObj[i].GetComponent<MonsterCol>().SetData(false);
            monstersObj[i].SetActive(value[i]);
        }
        bossObj.GetComponent<MonsterCol>().SetData(false);
        bossObj.SetActive(value[4]);
    }
    public void SetColForm_Monster(bool[] value)
    {
        monsterColForm.SetActive(true);
        for(int i = 0 ;i < 4 ; i ++)
        {
            monstersObj[i].GetComponent<MonsterCol>().SetData(true);
            monstersObj[i].SetActive(value[i]);
        }
        bossObj.SetActive(value[4]);
    }

    private void ResetForm()
    {
        for(int i = 0 ;i < 4 ; i ++)
        {
            monstersObj[i].GetComponent<MonsterCol>().TargetOff_();
            monstersObj[i].SetActive(false);
        }
        bossObj.SetActive(false);
        monsterAllObj.SetActive(false);
        knightsObj.SetActive(false);
        monsterColForm.SetActive(false);
        knightObj.SetActive(false);
        
        battle.kps[who].TargetOff();
        targetNum = -1;
    }
    
    #endregion

    #region SkillDrag 관련 / COL로 인한 스킬의 사용도 이곳에서
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
    
    
    public void EndDrag(Transform _skill)
    {
        if(targetNum== -1)
        {
            //누군가에게 col하지 않은채로 끝났다.
        }
        else
        {
            //스킬의 사용
            SkillData.Instance.UseSkill(skill.job, skill.skillNum, GetMyState(), GetState(targetNum));
            formObj[who].GetComponent<Form>().Cost -= skill.cost;
            formObj[who].GetComponent<Form>().UpdateText();
        }
        
        ResetForm();
        BattleUI.SetActive(true);
        SwapSkillHierarchy(invisibleSkill, _skill);
    }
    private State GetMyState()
    {
        return battle.kps[who].ks.s;
    }

    private State[] GetState(int num)
    {
        State[] states = new State[0];
        if(num <=3)
        {
            states = new State[1];
            states[0] = battle.kps[num].ks.s;
        }
        else if(num <=7)
        {
            states = new State[1];
            states[0] = battle.mps[num-4].s;
        }
        else if(num ==8)
        {
            states = new State[1];
            states[0] = battle.mps[4].s;
        }
        else if(num == 9)
        {
            states = new State[battle.knightTarget.Count];
            for(int i = 0 ; i< states.Length; i++)
                states[i] = battle.kps[battle.knightTarget[i]].ks.s;
            
        }
        else if(num == 10)
        {
            states = new State[battle.monsterTarget.Count];
            for(int i = 0 ; i< states.Length; i++)
                states[i] = battle.mps[battle.monsterTarget[i]-battle.knightCount].s;
        }
        else if(num == 11)
        {
            states = new State[1];
            states[0] = GetMyState();
        }

        return states;
    }
    
    int targetNum;
    public void SetSkillTarget(int i)
    {// i = -1 : 취소(Null), 0~3 : Knight , 4~7 : Monster,  8 : Boss, 9 : AllKnight,  10 : AllMonster, 11 : Me; 
        targetNum = i;
    }

    //대상이 되었던 몬스터의 Text를 업데이트한다.
    public void UpdateData()
    {
        if(targetNum >=0 && targetNum <=3)
        {
            battle.kps[targetNum].UpdateText();
        }
        else if(targetNum >=4 && targetNum <=8)
        {
            battle.msv[targetNum-4].UpdateText();
        }
        else if( targetNum == 9)
        {
            for(int i = 0 ; i < battle.knightTarget.Count; i++ )
                battle.kps[i].UpdateText();
        }
        else if(targetNum == 10)
        {
            for(int i = 0 ; i < battle.monsterTarget.Count; i++ )
                battle.msv[battle.monsterTarget[i]-battle.knightCount].UpdateText();
        }
    }
    #endregion
}
