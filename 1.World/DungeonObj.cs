using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonObj : MonoBehaviour
{
    public Dungeon d{get;set;}

    public void SetDungeon(Dungeon _d)
    {
        d = _d;
    }
}
