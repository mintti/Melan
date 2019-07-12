using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    #region PlayerData
    public int gold;
    public int day;
    #endregion
    
    public Text goldText;
    public Text dayText;


    public void SetData(int _day, int _gold)
    {
        gold = _gold;
        day = _day;

        UpdateText(); 
    }

    public void NextDay()
    {
        day++;
    }

    public void UpdateText()
    {
        dayText.text = string.Format("+{0}", day);
        goldText.text = string.Format("{0}", gold);
    }
    
}
