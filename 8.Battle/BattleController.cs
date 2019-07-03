using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Decoration deco;


    //던전 전투 데이타
    Dungeon dungeon;
    Knight[] team;
    Monster[] monsters;
    
    //던전 전투 데이타 GameController에서 호출.
    public void SetBattle(int _dungeon, int[] _team, int[] _monsters)
    {
        /*
        dungeon = _dungeon;
        team = _team;
        monsters = _monsters;
         */
    }
}
