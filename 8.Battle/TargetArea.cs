using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetArea : MonoBehaviour
{
    public Text targetText;
    public Image colImage; //명도관리로..
    public GameObject select;

    private bool isView;
    public void SetData()
    {
        isView = true;
        ColIsIn(false);
    }
    public void SetData(bool v)
    {   isView = false;
        colImage.color = new Color32(0, 0, 0,255);
        select.SetActive(false);
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

    #region 
    private void OnTriggerEnter2D(Collider2D other) {
        ColIsIn(true);
    }
    private void OnTriggerExit2D(Collider2D other) {
        ColIsIn(false);
    }
    private void ColIsIn(bool v)
    {
        if(!isView) return;
        colImage.color = v == true ? new Color32(0, 0, 0,255) : new Color32(100, 100, 100,255);
        select.SetActive(v);
    }
    #endregion
}
