using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

delegate void Del();

public class Work
{
    public int type;
    public int who;
    public int[] what;

    /*************************************
    * type = 0 : 전투    int party.num    int[] monster
    *        1 : 
    * 
    * 
    * 
     ************************************/

     public Work(int _type, int _who, int[] _what)
     {
         type = _type;
         who = _who;
         what = _what;
     }
}
public class EventManager : MonoBehaviour
{
        /*델리게이트 테스트
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
    */
    
    public List<Work> todayWork = new List<Work>();
    public List<Work> nextWork = new List<Work>();
    public void AddBattle(int who)
    {
        //Debug.Log(who);
    }

    #region World Work List관련
    public Text workText;
    public GameObject workPrefab;

    public void SetText()
    {
        workText.text = string.Format("{0}", todayWork.Count);
    }

    //1. Bubble버튼 클릭 시 List 출력
    public void MakeWorkList(Transform list)
    {
        CodeBox.ClearList(list);

        foreach(Work w in todayWork)
        {
            GameObject obj = CodeBox.AddChildInParent(list, workPrefab);
            obj.GetComponent<WorkPrefab>().SetData(w);
        }
    }

    //2. List에서 인자 클릭시 이벤트 실행.
    public void SelectWork()
    {

    }

    #endregion
}
