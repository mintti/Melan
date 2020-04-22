using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInfo : MonoBehaviour
{
    public int storeNum;
    public GameObject lockObj;

    public void Click()
    {
        GameController.Instance.town.npcTalk.On(storeNum);
    }

    public void Unlock(bool value)
    {
        lockObj.SetActive(!value);
    }
}
