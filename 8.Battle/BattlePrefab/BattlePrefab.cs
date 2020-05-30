using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePrefab : MonoBehaviour
{
    private Transform stateTr;//ks or ms
    private Transform skinTr;
    public void SetData()
    {
        //
    }
    
    
    #region  게임 진행
    public void TurnStart()
    {

    }

    public void TurnEnd()
    {

    }

    //UI
    public GameObject targetObj;
    public void TargetOn()
    {

    }
    public void TargetOff()
    {

    }
    #endregion

    #region 모션
    public void Motion(string motion)
    {
        skin.OrderMotion(motion);
    }
    public void Bubble(string bubble)
    {
        transform.SendMessage("SendBubble", bubble);
    }
    #endregion
}
