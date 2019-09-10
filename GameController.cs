using System.Collections;
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
    public EventManager event_;
    public ObjectController obj;

    public EventData eventData;
    public PlayerData player;

    void Start()
    {
       //Data 연결
        data = DataController.Instance;
        eventData = EventData.Instance;
        player = data.player;

        //초기 데이터
        obj.DungeonSearch();
        world.Click(); //초기 화면
        PlayerDataUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //UI에서 Player 정보가담긴 Text를 업데이트.
    public void PlayerDataUpdate()
    {
        obj.DayTextUpdate();
        obj.GoldTextUpdate();
    }


    public void LoadBattleScene()
    {
        eventData.SetBattleData(event_.selectWork);
        SceneManager.LoadScene("2.Battle");
    }


    //Castle Click
    public Button bt;
    public void ClickCastle()
    {
        bt.interactable  = EventData.Instance.todayWork.Count == 0 ?  true : false;
    }

     public void NextDay()
    {
        //이벤트 재생성
        event_.CreateWork();
        //버블 텍스트 변경.
        event_.SetText();

        //Day추가.        
        player.NextDay();
        
        //해당 데이터 저장
        //data.SaveOverlapXml();

    }

}
