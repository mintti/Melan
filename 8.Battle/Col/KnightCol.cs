using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCol : MonoBehaviour
{
    public int targetNum;
    private bool colActive;
    
    public void SetData(bool value)
    {
        if(!value)
        {
            TargetOn();
        }
        colActive = value;
    }

    
    private void OnTriggerEnter2D(Collider2D other) {
        if(!colActive) return;
        ColController.Instance.SetSkillTarget(targetNum);
        TargetOn();
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(!colActive) return;
        ColController.Instance.SetSkillTarget(-1);
        TargetOff();
    }

    public void TargetOn()
    {
        transform.GetComponent<BattleKnightPrefab>().TargetOn();
    }
    public void TargetOff()
    {
        transform.GetComponent<BattleKnightPrefab>().TargetOff();
    }
}
