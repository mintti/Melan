using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Check : MonoBehaviour
{
    public Text titleText;
    public Text bodyText;
    public Text tailText;

    public AdminNewKnight adminNewKnight;
    
    private string type;

    
    //확인 창을 보여줌.
    public void ShowInfo(string _type)
    {
        TextData text = TextData.Instance;
        UnitData unit  = UnitData.Instance;
        
        type = _type;
        switch(type)
        {
            case "고용" :
                int pay = unit.knightPay[unit.randomKnightList[adminNewKnight.Sig].level -1];
                SetText(string.Format("▶G {0}◀", pay), text.employment_Ment[0], text.employment_Ment[1]);
                break;

            default :
                break;
        }
    }

    private void SetText(string t1, string t2, string t3)
    {
        titleText.text = t1;
        bodyText.text = t2;
        tailText.text = t3;
    }

    //안내 창에서 확인을 누름
    public void IsCheck()
    {
        switch(type)
        {
            case "고용" :
            UnitData.Instance.Employment(adminNewKnight.Sig);
            adminNewKnight.DisableButton();//비활
            break;

            default :
            break;
        }
    }
}
