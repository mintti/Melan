using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCol : MonoBehaviour
{
    public DungeonInfoGuide guide;

    #region  Trigger영역
    void OnTriggerEnter2D(Collider2D col) {

        if(col.tag == "Dungeon")
        {
            Dungeon _d = col.GetComponent<Dungeon>();
            guide.gameObject.SetActive(true); 
            guide.SetData(_d);
        }
    }

    void OnTriggerExit2D(Collider2D col) {

        if(col.tag == "Dungeon")
        {
            Dungeon _d = col.GetComponent<Dungeon>();
            if(_d == guide.d)
                guide.gameObject.SetActive(false);    
        }
    }

    #endregion
}
