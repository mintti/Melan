using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCol : MonoBehaviour
{
    public int targetNum;
    private bool colActive;
    public Transform root;
    
    public void SetData(bool value)
    {
        if(!value)
        {
            TargetOn_();
        }
        colActive = value;
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        if(!colActive) return;
        ColController.Instance.SetSkillTarget(targetNum);
        TargetOn_();
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(!colActive) return;
        ColController.Instance.SetSkillTarget(-1);
        TargetOff_();
    }

    public void TargetOn_()
    {
        root.BroadcastMessage("TargetOn", transform,  SendMessageOptions.DontRequireReceiver);
    }
    public void TargetOff_()
    {
        root.BroadcastMessage("TargetOff", transform,  SendMessageOptions.DontRequireReceiver);
    }
}
