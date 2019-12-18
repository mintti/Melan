using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    private BattleMonsterPrefab bmp;
    public State state;
    BattleController battle;

    public int turn;
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
}
