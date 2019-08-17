using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCol : MonoBehaviour
{
    public UnitData unit;
    public DungeonInfoGuide guide;
    public GameObject DungeonExplan;
    public GameObject targetArrow;

    public Dungeon d{get;set;} //worldController에서 호출됨.
    private string[] ment{get;set;} = new string[3];

    private void Start()
    {
        unit = DataController.Instance.unit;
        ment = TextData.Instance.dungoenCol_Ment;
    }

    public void UpdateMent()
    {
        Text explan = DungeonExplan.transform.GetComponentInChildren<Text>();
        explan.text = ment[2];
    }

    #region  Trigger영역
    void OnTriggerEnter2D(Collider2D col) {

        if(col.tag == "Dungeon")
        {
            //화살표 생성
            GameObject obj = Instantiate(targetArrow);
            obj.transform.SetParent(col.transform, false);

            //해당 던전 정보를 guide에 전달.
            Dungeon _d = col.GetComponent<DungeonObj>().d;
            
            d = _d;
            guide.gameObject.SetActive(true); 
            guide.SetData(d);

            //던전 출전 가능 여부, 확인.
            DungeonExplan.SetActive(true);
            Text explan = DungeonExplan.transform.GetComponentInChildren<Text>();
            explan.text = ment[unit.GetPartyIndex(d.num) == -1 ? (_d.isAdmit() ? 0 : 1) : 2];

        }
    }

    void OnTriggerExit2D(Collider2D col) {

        if(col.tag == "Dungeon")
        {   
            //화살표 제거
            Destroy(col.transform.GetChild(1).gameObject);

            //지목된 던전과 guide의 타겟인 던전이 일치?
            Dungeon _d = col.GetComponent<DungeonObj>().d;
            if(_d == guide.d)
            {
                guide.gameObject.SetActive(false);    
                //DungeonExplan 상태 false전환.
                DungeonExplan.SetActive(false);
            }
        }
    }

    #endregion
}
