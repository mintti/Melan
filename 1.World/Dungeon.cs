using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dungeon : MonoBehaviour
{
    public int num;
    public string name;
    public int level;
    public int[] monsters;
    public int[] dangers;
    public int[] events;
    
    //보상
    public int gold;
    public int exper; 

    public Dungeon before;
    public GameObject next;

    public bool isClear;
    


    public bool isAdmit()
    {
        if(before == null || before.isClear == true) return true;
        else return false;
    }
}
