using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Form : MonoBehaviour
{
    private int cost; //행동력
    public BattleKnightPrefab bkp;
    public int Cost{get{return cost;} set{cost = value;}}
    public Skill[] skill;

    private State state;
    public void MyTurn()
    {
        Cost = 3;
    }
    public void SetData(BattleKnightPrefab _bkp)
    {   
        bkp = _bkp;
        state = bkp.ks.s;

        for(int i = 0; i < skill.Length; i++)
        {
            skill[i].SetData(bkp.ks, SkillData.Instance.GetSkill(bkp.ks.k.job, i));
        }
        /*
            0 기본 공격 1 방어
            2부터 표기된 스킬들. 2-2는 3의 값을 가짐.
         */
        //skill.Skill[0] = SkillData.Instance.GetSkill()

        
    }
}
