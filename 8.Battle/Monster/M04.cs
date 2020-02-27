using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M04 : MonoBehaviour
{
    public int num;

    public void UseSkill()
    {
        switch(num)
        {
            case 0 :
                Monster_0();
                break;
            case 1 :
                Monster_1();
                break;
            case 2 :
                Monster_2();
                break;
            case 3 :
                Monster_3();
                break;
            case 4 :
                Monster_4();
                break;
            default :
                break;
        }
    }


    private void Monster_0()
    {

    }
    private void Monster_1()
    {

    }
    private void Monster_2()
    {

    }
    private void Monster_3()
    {

    }
    private void Monster_4()
    {

    }
    /*
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
            case 2 :
                break;
            default:
                break;
        }

    }
    */

}
