using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectController : MonoBehaviour
{
    //던전 관련 
    public Transform dungeonObj;
    
    public void Strat()
    {
        DungeonSearch();
    }

    public void DungeonSearch()
    {
        DungeonData data = DungeonData.Instance;


        int i = 0;
        foreach(Transform tr in dungeonObj)
        {
            tr.GetComponent<DungeonObj>().SetDungeon(data.dungeons[i++]);
        }
    }
}
