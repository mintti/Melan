 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Form : MonoBehaviour
{
    private int cost; //행동력
    public int Cost{get{return cost;} set{cost = value;}}
    
    public Text costText;
    public BattleKnightPrefab bkp;
    public Skill[] skill;

    bool isOtherText;
    private State state;
    public void MyTurn()
    {
        Cost = 3;
        
        UpdateText();
    }

    public void SetData(BattleKnightPrefab _bkp)
    {   
        bkp = _bkp;
        state = bkp.ks.s;

        for(int i = 0; i < skill.Length; i++)
        {
            skill[i].SetData(bkp.ks, SkillData.Instance.GetSkillInfo(bkp.ks.k.job, i));
        }
        
        isOtherText = UnitData.Instance.useElementJob.Contains(bkp.ks.k.job) ? true : false;
    }

    public void UpdateText()
    {
        costText.text = System.Convert.ToString(cost);
        if(isOtherText) UpdateEleText();
    }

    public Text eleCostText;

    void UpdateEleText()
    {
        eleCostText.text = System.Convert.ToString(transform.GetComponent<Form>().bkp.ks.s.element.Cnt);
    }
}
