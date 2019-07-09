using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkPrefab : MonoBehaviour
{
    public Image icon;
    public Text text;

    public Work w{get;set;}

    public void SetData(Work _w)
    {
        w = _w;

        text.text = TextData.workList_Prefab_ment[w.type];
        icon.sprite = TextData.workList_Prefab_Image[w.type];
    }
}
