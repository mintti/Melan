using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldDungeonPrefab : MonoBehaviour
{
    private DungeonProgress dp;
    public int index;

    public Text nameText;
    public void SetData(DungeonProgress _dp, int _index)
    {
        dp = _dp;
        index = _index;
        UpdateText();    
    }

    public void Click()
    {
        GameController.Instance.world.SelectDungeon(index);
    }

    #region 그래픽
    public GameObject isEvent;
    public GameObject cloude10;
    public GameObject cloude30;
    public GameObject common50;
    public GameObject boss80;

    private Sprite[] img = new Sprite[3];

    public void UpdateText()
    {
        nameText.text = dp.d.name;
    
        cloude10.SetActive(dp.SearchP < 10 ? true : false);
        if(cloude10.active) cloude10.GetComponent<Image>().color =  DungeonData.Instance.CanGo(DungeonData.Instance.GetDungeonProgressIndex(dp)) ? new Color32(123, 115,115 ,255) : new Color32(0, 0,0 ,255);
        cloude30.SetActive(dp.SearchP < 30 ? true : false);
        common50.SetActive(dp.SearchP < 50 ? true : false);
        boss80.SetActive(dp.SearchP < 80 ? true : false);
    }

    //월컨 - DungeonUpdate()
    public void SetEvent(bool value)
    {
        isEvent.SetActive(value);
    }
    #endregion
    
}
