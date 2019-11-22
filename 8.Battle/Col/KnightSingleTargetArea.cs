using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSingleTargetArea : MonoBehaviour
{
    BattleController battle;
    public int targetNum;

    private bool isView;

    private void Awake() {
        battle = BattleController.Instance;
    }
    #region 
    private void OnTriggerEnter2D(Collider2D other) {
        battle.kps[ColController.Instance.who].TargetOn();
        ColController.Instance.SetSkillTarget(targetNum);
    }
    private void OnTriggerExit2D(Collider2D other) {
        battle.kps[ColController.Instance.who].TargetOff();
        ColController.Instance.SetSkillTarget(-1);
    }
    #endregion
}
