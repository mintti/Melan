using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;





//이벤트 1. 전투
public class Battle
{
    public Party p;
    public Dungeon d;
    public Monster[] m;

    public Battle(Party _p, Dungeon _d, Monster[] _m)
    {
        p = _p;
        d = _d;
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
    
    public ArrayList todayWork = new ArrayList();
    public ArrayList nextWork = new ArrayList();
    
    public Battle Bdata;//이멘트 보내기 전투 데이타.
}
