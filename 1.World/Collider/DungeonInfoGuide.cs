﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonInfoGuide : MonoBehaviour
{
    public GameObject guideObj; //가이드겸 출전 버튼    
    private Text guideText;

    public GameObject QmarkImg;
    public GameObject monsterList;
    public GameObject dangerList;


    public Text name;
    public Text level;

    public Dungeon d{get;set;}

    private void Start()
    {
        guideText = guideObj.GetComponentInChildren<Text>();
        this.gameObject.SetActive(false);
    }

    #region 던전 오브젝트와 충돌관련 이벤트.
    //던전 설명 팝업창. Set데이터.
    public void SetData(Dungeon _d)
    {
        d = _d;

        name.text = d.name;
        level.text = "";
        for(int i = 0; i < d.level; i++)
            level.text += "★";

        ListClear(monsterList);

        foreach(int i in d.monsters)
        {
            GameObject obj = Instantiate(QmarkImg);
            obj.transform.SetParent(monsterList.transform);

            //추후 클리어시 Qmark의 img(0번째 자식)의 sprite가 변경되게 해야함.
            /* 
            if(d.clear) << 이렇게?
            */
        }

        ListClear(dangerList);
        foreach(int i in d.dangers)
        {
            GameObject obj = Instantiate(QmarkImg);
            obj.transform.SetParent(dangerList.transform);
        }
    
    }
    
    public void SetActive(bool b)
    {
        this.gameObject.SetActive(b);
    }
    #endregion
    //ListClear
    private void ListClear(GameObject list)
    {
        foreach(Transform t in list.transform)
        {
            Destroy(t.gameObject);
        }
    }

    

}