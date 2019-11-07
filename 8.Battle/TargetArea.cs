using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetArea : MonoBehaviour
{
    public Text targetText;
    public Image colImage; //명도관리로..
        
    public void SetData()
    {

    }
    private void SetText(int who)
    {
        switch(who)
        {
            case 0 :
                targetText.text = "KNIGHT";
                break;
            case 1 :
                targetText.text = "COMMON";
                break;
            case 2 :
                targetText.text = "ELITE";
                break;
            case 3 :
                targetText.text = "BOSS";
                break;
            default:
                targetText.text = "ERROR";
                break;

        }
    }

    #region  트리거 영역 _ 프리팹 내 COL에서 호출됨.
    public void ColIsIn(bool v)
    {
        colImage.color = v == true ? new Color(0, 0, 0, 0.2f) : new Color(0, 0, 0, 0.6f);
    }
    #endregion
}
