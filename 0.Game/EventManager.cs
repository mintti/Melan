using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
public class EventManager : MonoBehaviour
{
    public List<Work> works = new List<Work>();
    public void AddBattle(int who)
    {
        //Debug.Log(who);
    }
}
