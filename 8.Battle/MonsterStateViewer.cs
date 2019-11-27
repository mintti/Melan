using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStateViewer : MonoBehaviour
{
    public BattleMonsterPrefab bmp;

    public void SetData(BattleMonsterPrefab _bmp)
    {
        bmp = _bmp;
        UpdateData();
    }

    public void UpdateData()
    {
        HpText();
    }

    //HP2
    public Text hpNowText;
    public Text hpTotalText;
    public RectTransform hpTr;

    void HpText()
    {   
        hpNowText.text  = System.Convert.ToString(bmp.ms.s.Hp); hpTotalText.text =  string.Format("/{0}", bmp.ms.m.hp);
        hpTr.sizeDelta = new Vector2(GetHpWidthSize() , hpTr.rect.height);
    }

    public float GetHpWidthSize()
    {
        float value = 100 * ((float)bmp.ms.s.Hp/(float)bmp.ms.m.hp);
        
        return value;
    }

    public void UpdateText()
    {
        HpText();
    }
}
