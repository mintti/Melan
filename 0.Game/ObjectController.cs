using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectController : MonoBehaviour
{

    //Player관련
    public Text gold;
    public Text day;

    public void GoldTextUpdate()
    {
        gold.text =  System.Convert.ToString(PlayerData.Instance.Gold);
    }
    public void DayTextUpdate()
    {
        day.text =  string.Format("+{0}", System.Convert.ToString(PlayerData.Instance.Day));
        }

    //던전 관련 
    public Transform dungeonObj;

    public void DungeonSearch() //Game에서 호출됨.
    {
        DungeonData data = DungeonData.Instance;


        int i = 0;
        foreach(Transform tr in dungeonObj)
        {
            tr.GetComponent<DungeonObj>().SetDungeon(data.dungeons[i++]);
        }
    }
}
