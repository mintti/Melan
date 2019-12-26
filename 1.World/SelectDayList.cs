using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDayList : MonoBehaviour
{
    public GameObject[] objs = new GameObject[5];
    public Text ment;
    
    public void SetData()
    {
        SelectDay(0);
    }
    public void SelectDay(int index)
    {
        for(int i = 0 ;i < 5; i++)
            ObjSetting(objs[i], i==index);
        ment.text = TextData.Instance.select_Day_Ment[index];

        GameController.Instance.world.dayIndex = index;
    }
    private void ObjSetting(GameObject obj, bool value)
    {
        obj.GetComponentInChildren<Text>().color = value ? new Color32(0, 0, 0, 255) : new Color32(125, 125, 125, 255);
        obj.GetComponent<Image>().color = value ? new Color32(236, 236, 236, 255) : new Color32(255, 255,255, 255);
    }
}
