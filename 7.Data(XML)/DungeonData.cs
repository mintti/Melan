using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dungeon
{
    public int num{get;set;}
    public string name{get;set;}
    public int level{get;set;}
    public int[] monsters{get;set;}
    public int[] dangers{get;set;}
    public int[] events{get;set;}
    public int day{get;set;}
    
    //보상
    public int gold{get;set;}
    public int exper{get;set;}

    public int before{get;set;}
    public int next{get;set;}

    public bool isClear{get;set;}


    public bool isAdmit()
    {
        if(DungeonData.Instance.dungeons[before] == null || 
            DungeonData.Instance.dungeons[before].isClear == true) return true;
        else return false;
    }
   

    public Dungeon(int _num, string _name, int _level, int[] _monster, int[] _dangers,
        int[] _events, int _day, int _gold, int _exper, int _before, int _next, bool _isClear)
    {
        num = _num;
        name = _name;
        level = _level;
        monsters = _monster;
        dangers = _dangers;
        events = _events;
        day = _day;
        gold = _gold;
        exper = _exper;
        before = _before;
        next = _next;
        isClear = _isClear;
    }
     
}

public class DungeonData : MonoBehaviour
{
    private static DungeonData _instance = null;

    public static DungeonData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(DungeonData)) as DungeonData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active DungeonData object");
                }
            }
            return _instance;
        }
    }


    public static void InstanceUpdata()
    {
        _instance = null;
    }

    public Dungeon[] dungeons{get;set;}

    public void ResizeDungeon(int size)
    {
        dungeons = new Dungeon[size];

    }
    
}
