using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M01_AnimaSlime : MonoBehaviour
{
    public void UseSkill()
    {
        MonsterBase mb = gameObject.GetComponent<MonsterBase>();
        State targetState;

        switch (mb.turn%2)
        {
            case 0 :
                targetState = BattleController.Instance.GetSingleTarget();
                targetState.AdDam(mb.state.Power);
                break;
            case 1 :
                targetState = BattleController.Instance.GetSingleTarget();
                targetState.AdDam(mb.state.Power);
                break;
            default:
                break;
        }

    }

}
