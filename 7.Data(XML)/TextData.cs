using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : MonoBehaviour
{

   private static TextData _instance;
   public static TextData Instance
   {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(TextData)) as TextData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active TextData object");
                }
            }
            return _instance;
        }
    }
    public string levelTail = "급";
    public string[] employment_Ment = new string[2]{
        "고용 시 해당 금액이 빠져나가고, 30턴 후 다시 지급합니다.",
        "지급하지 않을 경우 계약이 해지됩니다."};


    public string[] job_lan = new string[11]{"전사" , "마법사", "도적" , "미정", 
                                            "미정", "미정", "미정", "미정",
                                            "미정", "미정", "미정"};

    #region WORLD
    public string[] workList_Button_Ment_Text = new string[9]{"탐색", "전투", "미정", "미정", "미정", "미정", "미정", "수령", "귀환"};
    public string[] workList_Info_Ment_Text = new string[9]{"원하는 기사들로 파티를 이뤄 던전으로 탐색을 보냅니다", "전투가 발생했습니다", "ㅁ", "ㅁ", "ㅁ", "ㅁ", "ㅁ", "일정대로 탐색완료했습니다. 보상을 수령하세요", "파티를 강제귀환시킬 수 있습니다"};

    public string[] select_Day_Ment = new string[5]{
        "탐색률 + 5%", "탐색률 + 10%, 추가 골드 + 5%", "탐색률 + 15%, 추가골드 +8%", "탐색률 + 20%, 추가골드 15%", "탐색률 + 30%, 추가골드 + 25%"
    };
    #endregion    

}
