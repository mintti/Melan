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
            bt1.GetComponent<Text>().text = TextData.Instance.choice_Event_Answer[type, 0];
        }
        else//답변이 두개임
        {
            bt1.GetComponent<Text>().text = TextData.Instance.choice_Event_Answer[type, 0];
            bt2.GetComponent<Text>().text = TextData.Instance.choice_Event_Answer[type, 1];
        }
    }

    public void Choice(int value)
    {
        switch (type)
        {
            case 0 :

                break;
            case 1 :

                break;
            case 2 :
                
                break;
            case 3 :
                
                break;
            default:
                
                break;
        }
    }

    
}
