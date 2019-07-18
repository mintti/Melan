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
   
   public string[] dungoenCol_Ment{get;set;} = new string[3];
   //WorkPrefab
   public string[] workList_Prefab_ment{get;set;} = new string[3];
   public Sprite[] workList_Prefab_Image{get;set;} = new Sprite[3];

   public void setTextData()
   {
      dungoenCol_Ment[0] = string.Format("출전 가능. 이곳을 눌러 준비해주세요.");
      dungoenCol_Ment[1] = string.Format("출전 불가능. 이전 던전 클리어 필요.");
      dungoenCol_Ment[2] = string.Format("출전 불가능. 토벌간 파티 존재.");

      workList_Prefab_ment[0] = string.Format("출전 중 몬스터 발견!");
      workList_Prefab_ment[1] = string.Format("테스트 멘트2");
      workList_Prefab_ment[2] = string.Format("테스트 멘트3");
   }
   
}
