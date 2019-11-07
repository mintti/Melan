using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Form;
public class WizardForm : MonoBehaviour
{
    private int myCost;
    private int myCost_Element;

    public Form.Skill[] skill= new Form.Skill[15];
    private void Start() {
        
    }


    void SetData()
    {   /*
            0 기본 공격 1 방어
            2부터 표기된 스킬들. 2-2는 3의 값을 가짐.
         */
        //skill.Skill[0] = SkillData.Instance.GetSkill()
    }

    
}
