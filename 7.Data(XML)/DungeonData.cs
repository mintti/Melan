using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonData : MonoBehaviour
{
    
    public Transform Aragaya;
    public Transform Benuxu;

    public Dungeon[] dungeons;

    public void Test_ConnectData()
    {
        int cnt = Aragaya.childCount + Benuxu.childCount;
        Array.Resize(ref dungeons, cnt+1);

        cnt = 1;

        foreach(Transform tr in Aragaya)
        {
            dungeons[cnt++] = tr.GetComponent<Dungeon>();
        }

        foreach(Transform tr in Benuxu)
        {
            dungeons[cnt++] = tr.GetComponent<Dungeon>();
        }
    }


}
