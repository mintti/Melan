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
    
    

    public void SetData<T>(T e)
    {
        switch(typeof(T).Name)
        {
            case "Battle" :
                text.text = TextData.Instance.workList_Prefab_ment[0];
                //icon.sprite = TextData.workList_Prefab_Image[w.type];
                break;
            default :
                Debug.Log("입력받은 class에 해당하는 유형이 없음.");
                break;
        }
    }

    public void Click()
    {
        parent.SelectWork(transform);
    }
}
