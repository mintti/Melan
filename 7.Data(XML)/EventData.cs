using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Work
{
    public int type;
    public int who;
    public int[] what;

    /*************************************
    * type = 0 : 전투    int party.num    int[] monster
    *        1 : 
    * 
    * 
    * 
     ************************************/

     public Work(int _type, int _who, int[] _what)
     {
         type = _type;
         who = _who;
         what = _what;
     }
     public void What(int[] arr)
     {
        Array.Resize(ref what, arr.Length);
        what = arr;
     }
}


public class EventData : MonoBehaviour
{
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
    
}
