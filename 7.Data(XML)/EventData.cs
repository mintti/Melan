using System.Collections;
using System.Collections.Generic;
using UnityEngine;

delegate void Del();

public class EventData : MonoBehaviour
{
    Del todo;

    public static void D_battle()
    {
        Debug.Log("전투");
    }
    public static void D_day()
    {
        Debug.Log("정규 행사");
    }


    public void Test_InsertData()
    {
        todo = new Del(D_battle);
        todo += D_day;
    }

    public void Test_OutputData()
    {
        todo();
    }



    #region 
    

    #endregion
}
