using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminController : MonoBehaviour
{   
    public GameObject admin;
    public GameObject knightList; //아래의 Prefab이 생성될 위치.
    public GameObject knightPrefab; //admin - knight 객체.

    public UnitData unit;
    


    //KightList생성
    public void MakeKinghtList(Transform list)
    {
        CodeBox.ClearList(list);

        foreach(Knight k in unit.knights)
        {
            GameObject obj = CodeBox.AddChildInParent(list, knightPrefab);
            obj.GetComponent<KnightPrefab>().SetData(k);
        }
    }

    //KightList 데이터 업데이트.
    public void KnightListUpdate()
    {
        
    }

    public void AdminOn()
    {
        
    }

    
}
