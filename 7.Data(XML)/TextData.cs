using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : MonoBehaviour
{

   public DungeonCol DungeonCol;
   
   //WorkPrefab
   public static string[] workList_Prefab_ment = new string[3];
   public static Sprite[] workList_Prefab_Image = new Sprite[3];

   public void setTextData()
   {
      DungeonCol.ment[0] = string.Format("출전 가능. 이곳을 눌러 준비해주세요.");
      DungeonCol.ment[1] = string.Format("출전 불가능. 이전 던전 클리어 필요.");
      DungeonCol.ment[2] = string.Format("출전 불가능. 토벌간 파티 존재.");

      workList_Prefab_ment[0] = string.Format("출전 중 몬스터 발견!");
      workList_Prefab_ment[1] = string.Format("테스트 멘트2");
      workList_Prefab_ment[2] = string.Format("테스트 멘트3");
   }
   
}
