using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkPrefab : MonoBehaviour
{
    public Image icon;
    public Text text;

    public Work w{get;set;}


    public EventManager parent{get;set;}
    private string[] ment = new string[3];
    
    public void SetData(Work _w)
    {
        w = _w;

        text.text = TextData.workList_Prefab_ment[w.type];
        //icon.sprite = TextData.workList_Prefab_Image[w.type];
    }

    public void Click()
    {
        parent.SelectWork(w);
    }
}
