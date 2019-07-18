using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public DataController data;
    public WorldController world;
    public AdminController admin;
    public EventManager event_;
    public ObjectController obj;

    public EventData eventData;
    public PlayerData player;



    /* 
    public static GameObject gameInstance = null;
    public static GameObject unitInstance = null;
    */
    
    void Start()
    {
        /*
        if(gameInstance!=null)
        {
            Destroy(gameInstance);
            Destroy(unitInstance);
        }
        */
        StartSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartSetting()
    { 
        
        data.LoadResource();
        DataController.InstanceUpdata();
        data.Test_InsertData();
        data.LoadXml("SecondData");
        /*
        //데이타 로드
        if(DataController.Instance == null)
        {
            data.Test_InsertData();
            data.LoadXml("SecondData");
            Debug.Log("첫 로드");
        }
        else
        {
            data  = DataController.Instance;
            eventData = EventData.Instance;
            player = PlayerData.Instance;
            Debug.Log("기존 데이터 로드");

            obj.DungeonSearch();
        }
        //
        world.Click();
        event_.SetText();
     */
    }

    public void Test_LoadScene()
    {
        //DontDestroyOnLoad(this);
        DataController.InstanceUpdata();
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
        event_.ChangeWork();
        //버블 텍스트 변경.
        event_.SetText();

        //Day추가.        
        player.NextDay();
        player.UpdateText();

        //해당 데이터 저장
        //data.SaveOverlapXml();

    }

}
