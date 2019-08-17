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

    private void Start() {
         DungeonSearch();
    }
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


    /*  던전 컨넥트..
        던전 연결 기준은 그 리스트에서 순서대로 1....N이다.
        고로 순서에 알맞도록 잘 배치해주어야한다.
                    */
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
