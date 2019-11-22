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
        state.SetData(10, 10, 5, 0, null, LifeType.M);
    }

    public void UseSkill()
    {
        switch (turn%2)
        {
            case 0 :
                State targetState = battle.GetSingleTarget();
                targetState.AdDam(state.Power);
                break;
            case 1 :
                targetState = battle.GetSingleTarget();
                targetState.AdDam(state.Power);
                break;
            default:
                break;
        }

        battle.NextTurn();
    }

}
