using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleKnightPrefab : MonoBehaviour
{
    public KnightState ks;

    public KnightSkinPrefab skin;
    public int index;
    //해당스크립트 활성화 여부임. SetData()를 통해 활성화됨.
    public bool isKnight;
    
    
    //BattleCon - MonsterSetting()에서 호출됨.
    public void SetData(KnightState _ks, int n)
    {
        ks = _ks;
        
        skin.SetData(ks.k);
        isKnight = true;
        UpdateText();
        TurnEnd();
        index = n;
    }
    
    public void Die()
    {
        ks.s.Die();
    }
    #region  게임 진행
    public void TurnStart()
    {
        myTurnObj.SetActive(true);
        transform.SendMessage("SendBubble", "나의 턴이군~");
    }

    public void TurnEnd()
    {
        TargetOff();
        myTurnObj.SetActive(false);
    }

    #endregion
    #region 효과(UI)
    //정보로드
    public GameObject infoObj;

    public Text hpText;
    public Text powerText;
    public Text speedText;
    public Text stressText;

    //HP2
    

    //Text 관련
    public void UpdateText()
    {
        HpText();
        SetStressText();

        PowerText();
        SpeedText();
        StressText();
    }

    public void PowerText()
    {   powerText.text = string.Format("{0}", ks.s.Power);}
    public void SpeedText()
    {   speedText.text = string.Format("{0}", ks.s.Speed);}
    public void StressText()
    {   stressText.text = string.Format( "{0}%", ks.s.Stress);} 
    
    //인포
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
    //타겟 관련.
    public GameObject myTurnObj;

    //Skill TargetEffect
    public GameObject targetObj;
    public void TargetOn()
    {
        targetObj.SetActive(true);
    }
    public void TargetOff()
    {
        targetObj.SetActive(false);

    }

    //체력바
    public Text hpNowText;
    public Text hpTotalText;
    public RectTransform hpTr;
    public void HpText()
    {   hpText.text = string.Format("{0}/{1}", ks.s.Hp, ks.k.hp);
        hpNowText.text  = System.Convert.ToString(ks.s.Hp); hpTotalText.text =  string.Format("/{0}", ks.k.hp);
        hpTr.sizeDelta = new Vector2(GetHpWidthSize() , hpTr.rect.height);
    }
    private float GetHpWidthSize()
    {
        float value = 160 * ((float)ks.s.Hp/(float)ks.k.hp);
        return value;
    }
    
    public Text stressNowText;
    public RectTransform stressTr;
    public void SetStressText()
    {
        stressNowText.text = string.Format("{0}%", ks.s.Stress);
        stressTr.sizeDelta = new Vector2(GetStressWidthSize() , stressTr.rect.height);
    }
    
    private float GetStressWidthSize()
    {
        float vlaue = 160 * ((float)ks.s.Stress / 100f);
        return vlaue;
    }
    #endregion

    
}
