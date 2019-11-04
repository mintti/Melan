using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleKnightPrefab : MonoBehaviour
{
    public KnightState ks;

    public KnightSkinPrefab skin;
    
    //해당스크립트 활성화 여부임. SetData()를 통해 활성화됨.
    public bool isKnight;
    
    
    //BattleCon - MonsterSetting()에서 호출됨.
    public void SetData(KnightState _ks)
    {
        ks = _ks;
        
        skin.SetData(ks.k.skin);
        isKnight = true;
        UpdateText();
    }
    
    //정보로드
    public GameObject infoObj;

    public Text hpText;
    public Text powerText;
    public Text speedText;
    public Text stressText;

    public void UpdateText()
    {
        HpText();
        PowerText();
        SpeedText();
        StressText();
    }

    public void HpText()
    {   hpText.text = string.Format("{0}/{1}", ks.s.Hp, ks.k.hp);}
    public void PowerText()
    {   powerText.text = string.Format("{0}", ks.s.Power);}
    public void SpeedText()
    {   speedText.text = string.Format("{0}", ks.s.Speed);}
    public void StressText()
    {   stressText.text = string.Format( "{0}%", ks.s.Stress);}

    public void Click()
    {
        if(isKnight)
        {
            if(infoObj.active)
            {
                infoObj.SetActive(false);
                return;
            }
            infoObj.SetActive(true);  
        }
    }

    public void TurnStart()
    {
        FadeIn();
    }

    public void TurnEnd()
    {
        FadeOut();
    }
    #region 효과
    public RawImage rawImage;
    private void FadeOut()
    {
        rawImage.color = new Color(150, 150, 150);
    }
    private void FadeIn()
    {
        rawImage.color = new Color(255, 255, 255);
    }
    #endregion


    #region 모션

    #endregion
}
