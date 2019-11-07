using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Form;
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
    */
    public GameObject BattleUI;

    public GameObject knightArea_ALL;
    public GameObject[] knightArea_4 = new GameObject[4];

    public GameObject monsterArea_ALL;
    public GameObject monsterArea_BOSS;
    public GameObject[] monsterArea_4 = new GameObject[4];
    public GameObject[] monsterArea_HALF_4 = new GameObject[4];
    public GameObject[] monsterArea_HALF_2 = new GameObject[2];
    
    public Sprite testSkillspr;

    bool isKnight;

    //각 Form.Skill에서 호출된다.
    public void SetData(Form.Skill skill)
    {
        //대상이 아군이야?
        isKnight = skill.target == Form.Target.MINE ||  skill.target == Form.Target.WE ? true : false ;
        BattleUI.SetActive(false);

    }

    //입력 받은 SKill을 분석하여 알맞게 폼을 변경해준다.
    public void ChangeColForm(string type)
    {
        
    }

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
    public void EndDrag(Transform skill)
    {
        BattleUI.SetActive(true);
        SwapSkillHierarchy(invisibleSkill, skill);
    }
    #endregion
}
