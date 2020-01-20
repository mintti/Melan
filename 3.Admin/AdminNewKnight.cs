using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminNewKnight : MonoBehaviour
{
    public Transform NewKnightList;
    public KnightSkinPrefab[] knightSkinPrefab = new KnightSkinPrefab[3];
    
    public NewKnight[] newKnights = new NewKnight[3];
    public Button employBt;

    public void SetData()
    {
        RandomKnight[] randomKnights = UnitData.Instance.randomKnightList;
        for(int i = 0; i < 3; i ++)
        {
            newKnights[i].SetData(randomKnights[i] ,i);
            knightSkinPrefab[i].SetData(new Knight(randomKnights[i]));
        }

        employBt.interactable = false;
    }
    private int sig;
    public int Sig
    {
        get{
            return sig;
        }   
        set
        {
            sig = value;
            employBt.interactable = true;
        }
    }
    //NewKnight -  Click()에서 호출됨.
    public void ClickSignal(int _sig)
    {
        Sig = _sig;
        for(int i = 0 ; i < newKnights.Length; i ++)
        {
            if (i == Sig)
                newKnights[i].Check();
            else
                newKnights[i].NonCheck();
        }
    }

    //Check - UnitEpoly 시 버튼(고용된 기사, 고용버튼) 비활 및 NonCheck 화
    public void DisableButton()
    {
        newKnights[Sig].Employment();
        employBt.interactable = false;
    }
}

