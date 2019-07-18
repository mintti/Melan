using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldKnightPrefab : MonoBehaviour
{
    #region  Obj Part
    public WorldController world;

    public Image skin;
    public GameObject check;
    public GameObject teaming;
    #endregion

    public Knight k{get;set;}
    private int isType{get;set;} = 0;  // 0 : 선택가능 기사리스트 , 1 : 선택 불가 블럭 타입(현제 팀 출전중.),  2: 선택 시 제거됨(셀렉트뷰)

    void Start()
    {
        check.SetActive(false);
        world = GameObject.FindWithTag("GameController").GetComponent<WorldController>();
    }


    //아래 출전기사들 리스트에 들어감.
    public void KngihtInSelectView(Knight k)
    {
        SetData(k);
        IsType(2);
    }


    public void SetData(Knight _k)
    {
        
        k = _k;
        skin.sprite = k.skin;

        //출전 중인 기사들은..
        if(k.teaming) 
        {
            IsType(1);
        }

    }

    //Obj Button 클릭시 호출됨.
    public void Check()
    {
        if(isType == 0)
        {
            check.SetActive(check.activeSelf ? false : true);

            if(check.activeSelf)    world.AddKnightInParty(k);
            else    world.RemoveKnightInParty(k);
        }
        else if(isType == 2)
        {
            world.RemoveKnightInParty(k);
        }
    }

    public void IsType(int i)
    {
        switch(i)
        {
            case 0 :
                isType = 0;
                check.SetActive(false);
                //teaming.SetActive(false);
                break;
            case 1 : 
                isType = 1;
                teaming.SetActive(true);
                break;
            case 2 :
                isType = 2;
                break;
            default :
                break;
        }
    }


}
