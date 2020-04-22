using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceEvent : MonoBehaviour
{
    public Text text;
    public Image image;
    public GameObject bt1;
    public GameObject bt2;
    public GameObject bt3;
    
    //결과찰
    public GameObject result_Obj;

    public Sprite[] event_Sprite_Array = new Sprite[4];
    
    private DungeonProgress dp;
    private int type;
    List<int> answer_1 = new List<int>(){1};//한가지 선택지를 가지는 이벤트으 인텍스
    public void SetData(int index)
    {
        gameObject.SetActive(true);
        dp = DungeonData.Instance.dungeon_Progress[index];
        type = dp.choice_Event_Type;

        //화면 세팅
        text.text = TextData.Instance.choice_Event_Ment[type];
        image.sprite = event_Sprite_Array[type];
        
        bt2.SetActive(true);
        if(answer_1.Contains(type))
        {
            bt2.SetActive(false);
            bt1.GetComponentInChildren<Text>().text = TextData.Instance.choice_Event_Answer[type, 0];
        }
        else//답변이 두개임
        {
            bt1.GetComponentInChildren<Text>().text = TextData.Instance.choice_Event_Answer[type, 0];
            bt2.GetComponentInChildren<Text>().text = TextData.Instance.choice_Event_Answer[type, 1];
        }
    }

    public void Choice(int value)
    {
        bool result = Random.Range(0, answer_1.Contains(type) ? 1 : 2 ) == 0 ? true : false;
        int answer = 0;
        int dp_Evnet_Type_Num = 8;
        switch (type)
        {
            case 0 ://Left_Or_Right
                int day = DungeonData.Instance.day_Array[dp.p.dayIndex] / 2;
                dp.p.Day += result ? -day : day/2 ;
                answer = result ? 0 : 1;
                break;
            case 1 ://Lucky_Lake
                foreach(int k in dp.p.k)
                    UnitData.Instance.knights[k].Lucky_Lake();
                answer = 2;
                break;
            case 2 ://Treasure_Chest
                if(value == 1 )
                {
                    answer = 5;
                    break;
                }
                if(result)
                    dp.Reward += 100;
                /*
                else// 이건 미믹 전투화면이된다. 
                    {
                        //dp_Evnet_Type_Num = 5;
                    }
                */
                answer = result ? 3 : 4 ;
                break;
            case 3 ://Gamble
                break;
            default:
                break;
        }
        //표시
        result_Obj.SetActive(true);
        result_Obj.GetComponentInChildren<Text>().text = TextData.Instance.choice_Event_Result[answer];

        dp.SetData(dp_Evnet_Type_Num);
        
        GameController.Instance.EventCheck();
    }

    public void Close()
    {
        GameController.Instance.world.DungeonUpdate();
        result_Obj.SetActive(false);
        gameObject.SetActive(false);
    }

}
