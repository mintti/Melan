using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminNewKnight : MonoBehaviour
{
    public Transform NewKnightList;
    public KnightSkinPrefab[] knightSkinPrefab = new KnightSkinPrefab[3];
    
    public NewKnight[] newKnights = new NewKnight[3];

    public void SetData()
    {
        RandomKnight[] randomKnights = UnitData.Instance.randomKnightList;
        for(int i = 0; i < 3; i ++)
        {
            newKnights[i].SetData(randomKnights[i] ,i);
            knightSkinPrefab[i].SetData(new Skin(randomKnights[i].skinNum));
        }
    }
    public int sig;
    //NewKnight -  Click()에서 호출됨.
    public void ClickSignal(int _sig)
    {
        sig = _sig;
        for(int i = 0 ; i < newKnights.Length; i ++)
        {
            if (i == sig)
                newKnights[i].Check();
            else
                newKnights[i].NonCheck();
        }
    }
}

