using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Form : MonoBehaviour
{
    public GameObject[] skills = new GameObject[5]; 
    public Text skillInfoText;
    
    private BattleKnightPrefab bkp;

    public void SetData(BattleKnightPrefab _bkp)
    {
        bkp = _bkp;

        for(int i = 0 ; i < 5 ; i ++)
            skills[i].GetComponent<Skill>().SetData( this, bkp.ks, SkillData.Instance.GetSkillInfo(bkp.ks.k.job, i));
    }

    public void SetInfoText(string text)
    {
        skillInfoText.text = text;
    }
    
    public void NextTurn()
    {
        BattleController.Instance.NextTurn();
    }
}
