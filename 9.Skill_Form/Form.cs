using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Form : MonoBehaviour
{
    private int myCost;
    private int myCost_Element;

    public Skill[] skill;

    public void MyTurn()
    {

    }
    public void Test()
    {
        Debug.Log("출력됨");
    }

    void SetData()
    {   /*
            0 기본 공격 1 방어
            2부터 표기된 스킬들. 2-2는 3의 값을 가짐.
         */
        //skill.Skill[0] = SkillData.Instance.GetSkill()
    }
}
