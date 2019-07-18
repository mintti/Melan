using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
     private static PlayerData _instance;
    public static PlayerData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(PlayerData)) as PlayerData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active PlayerData object");
                }
            }
            return _instance;
        }
    }


    #region PlayerData
    private int gold = 0;
    private int day = 0;

    public int Gold{
        get{ return gold; }
        set{ gold = value;
            UpdateText(); }
    }
    public int Day{
        get{ return day;}
        set{ day = value; 
            UpdateText(); }
    }
    #endregion
    
    public Text goldText;
    public Text dayText;



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
