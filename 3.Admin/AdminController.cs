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
    public void MakeKinghtList()
    {
        foreach(Transform t in knightList.transform)
        {
            Destroy(t.gameObject);
        }

        foreach(Knight k in unit.knights)
        {
            GameObject obj = Instantiate(knightPrefab);
            obj.transform.SetParent(knightList.transform); //부모-자식 지정.
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
