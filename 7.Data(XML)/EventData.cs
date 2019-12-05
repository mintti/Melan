using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//이벤트 0. 메인(연결자)
public class Work
{    
    /* 
        0 : 전투


     */
    public int type;
    public int index;

    public Work(int _t, int _i)
    {
        type = _t;
        index = _i;
    }
}
//이벤트 1. 전투
[System.Serializable]
public class Battle
{
    public Party p;
    public int[] m; //Monster

    public Battle(Party _p, int[] _m)
    {
        p = _p;
        m = _m;
    }
}


public class EventData : MonoBehaviour
{
    delegate void Del();//델리게이트 뭐 선언.. ? 

    private static EventData _instance;
    public static EventData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(EventData)) as EventData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active EventData object");
                }
            }
            return _instance;
        }
    }
    
    public List<Work> todayWork = new List<Work>();
    public List<Work> nextWork = new List<Work>();
    
    public List<Battle> battles = new List<Battle>();
    public Battle Bdata;//이멘트 보내기 전투 데이타.

    public void Reset()
    {
        todayWork.Clear();
        battles.Clear();
    }

    public void SetBattleData(Work w)
    {
        int n = w.index;
        Bdata = battles[n];
    }

    public void CompleteBattle(Battle b)
    {
        //Battle 씬에서 수행.

        //보상 누적과
        //battles리스트, XmlData 제거
    }
}
