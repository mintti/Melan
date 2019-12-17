﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(GameController)) as GameController;

                if(_instance == null)
                {
                    Debug.LogError("There's no active GameController object");
                }
            }
            return _instance;
        }
    }


    public DataController data;
    public WorldController world;
    public AdminController admin;

    public EventData event_;
    public PlayerData player;

    //DataController - LoadDataProcess(2)에서 호출됨.
    public void ConnectData()
    {
        data = DataController.Instance;
        event_ = EventData.Instance;
        player = data.player;

        world.Click(); //초기 화면
        PlayerDataUpdate();
        world.CreateDungeonInWorld();
        event_.Connect_DungeonEvent_And_Party();
    }

    #region  Player Data
    public Text gold;
    public Text day;

    public void GoldTextUpdate()
    {
        gold.text =  System.Convert.ToString(PlayerData.Instance.Gold);
    }
    public void DayTextUpdate()
    {
        day.text =  string.Format("+{0}", System.Convert.ToString(PlayerData.Instance.Day));
        }
    //UI에서 Player 정보가담긴 Text를 업데이트.
    public void PlayerDataUpdate()
    {
        GoldTextUpdate();
        DayTextUpdate();
    }
    

    #endregion
    
    public void LoadScene(int num)
    {
        switch(num)
        {
            case 0 :
                SceneManager.LoadScene("0.Start");
                break;
            case 1:
                SceneManager.LoadScene("1.Main");
                break;
            case 2: 
                SceneManager.LoadScene("2.Battle");
                break;
            case 3:
                break;
            default :
                break;
        }
    }

    //Castle Click
    public Button bt;

    public void NextDay()
    {
        if(event_.Check_RemainingEvent())
            return;
        UnitData.Instance.NextDay();
        //이벤트 재생성
        event_.CreateEvent();

        //다음 시간.  
        player.NextDay();
        
        //해당 데이터 저장
        //data.SaveOverlapXml();
        data.SaveXml();
        world.DungeonUpdate();
    }
}
