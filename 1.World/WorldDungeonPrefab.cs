using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldDungeonPrefab : MonoBehaviour
{
    private DungeonProgress dp;
    public Text nameText;
    public Text saturationText; //포화도 텍스트
    public Text searchPText; //탐색률 텍스트
    public GameObject cloude;

    private Sprite[] img = new Sprite[3];

    public void SetData(DungeonProgress _dp)
    {
        dp = _dp;
        UpdateText();    
    }

    public void UpdateText()
    {
        nameText.text = dp.d.name;
        saturationText.text = string.Format("{0}%", dp.Saturation);
        searchPText.text =  string.Format("{0}%",dp.SearchP );
    
        cloude.SetActive(dp.SearchP < 10 ? true : false);
    }
}
