using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightInfo_Simple : MonoBehaviour
{
    public Knight k{get;set;}

    #region 이미지
    public RawImage img;

    public void SetCharacter(Texture txr)
    {
        img.texture = txr;
    } 
    #endregion
    
    public Text hpText;
    public Text stressText;


    public void SetData(Knight _k)
    {
        k  = _k;

        hpText.text = string.Format("{0}%" , (int)((float)k.maxHp/k.hp * 100));
        stressText.text = string.Format("{0}%", k.stress);
    }

}
