using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M01_Slime : MonoBehaviour
{
    private BattleMonsterPrefab bmp;
    State state;
    BattleController battle;

    int turn;
    private void Start()
    {
        battle = BattleController.Instance;
        bmp = this.transform.GetComponent<BattleMonsterPrefab>();
        state = bmp.s;
        turn = battle.Turn;
    }

    public void SetData()
    {
        state.SetData(bmp.ms);
    }

    public void UseSkill()
    {
        State targetState;
        switch (turn%2)
        {
            case 0 :
                targetState = battle.GetSingleTarget();
                targetState.AdDam(state.Power);
                break;
            case 1 :
                targetState = battle.GetSingleTarget();
                targetState.AdDam(state.Power);
                break;
            default:
                break;
        }

    }

}
