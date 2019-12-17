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
        set{
            gold = value;
            GameController.Instance.PlayerDataUpdate();
            DataController.Instance.UpdateGold();
            }
    }
    public int Day{
        get{ return day;}
        set{
            day = value; 
            GameController.Instance.PlayerDataUpdate();
            DataController.Instance.UpdateDay();
            }
    }
    #endregion

    private int stress;
    public int Stress{
        get{return stress;}
        set{
            stress = value;
            DataController.Instance.UpdateGold();
            }
    }

    public void NextDay()
    {
        Day++;
        UnitData.Instance.CreateRandomKnight(3);
    }
    
    private int level; //1~3
    public int Level{get{return level;} set{level = value;}}

}
