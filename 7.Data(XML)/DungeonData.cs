using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonData : MonoBehaviour
{
    
    public Transform Aragaya;
    public Transform Benuxu;

    public List<Dungeon> dungeons = new List<Dungeon>();

    public void Test_ConnectData()
    {
        foreach(Transform tr in Aragaya)
        {
            dungeons.Add(tr.GetComponent<Dungeon>());
        }

        foreach(Transform tr in Benuxu)
        {
            dungeons.Add(tr.GetComponent<Dungeon>());
        }
    }


}
