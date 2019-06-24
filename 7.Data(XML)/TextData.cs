using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : MonoBehaviour
{

    public DungeonCol DungeonCol;

   
   public void setTextData()
   {
       DungeonCol.ment[0] = "출전 불가능. 이전 던전 클리어 필요.";
       DungeonCol.ment[1] = "출전 가능. 이곳을 눌러 준비해주세요.";
   }
}
