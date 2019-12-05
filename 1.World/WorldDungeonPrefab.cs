using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldDungeonPrefab : MonoBehaviour
{
    private DungeonProgress dp;
    public int index;

    public Text nameText;
    public Text saturationText; //포화도 텍스트
    public Text searchPText; //탐색률 텍스트

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
    public GameObject cloude10;
    public GameObject cloude30;
    public GameObject common50;
    public GameObject boss80;

    private Sprite[] img = new Sprite[3];

    public void UpdateText()
    {
        nameText.text = dp.d.name;
        saturationText.text = string.Format("{0}%", dp.Saturation);
        searchPText.text =  string.Format("{0}%",dp.SearchP );
    
        cloude10.SetActive(dp.SearchP < 10 ? true : false);
        cloude30.SetActive(dp.SearchP < 30 ? true : false);
        common50.SetActive(dp.SearchP < 50 ? true : false);
        boss80.SetActive(dp.SearchP < 80 ? true : false);
    }
    #endregion
    
}
