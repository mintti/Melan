using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkPrefab : MonoBehaviour
{
    public Image icon;
    public Text text;

    public EventManager parent{get;set;}
    private string[] ment = new string[3];
    
    public Work w{get;set;}

    public void SetData(Work _w)
    {
        w = _w;
        switch(w.type)
        {
            case 0 :
                text.text = TextData.Instance.workList_Prefab_ment[0];
                //icon.sprite = TextData.workList_Prefab_Image[w.type];
                break;
            default :
                Debug.Log("입력받은 work에 해당하는 WorkType유형이 없음.");
                break;
        }
    }

    

    public void Click()
    {
        parent.SelectWork(transform);
    }
}
