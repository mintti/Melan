using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownController : MonoBehaviour
{
    public GameObject townObj;
    public void TownOn()
    {
        townObj.SetActive(true);
        SetStore();
    }
    public StoreInfo[] storeList = new StoreInfo[3];
    //Store에 Unlock 여부
    public void SetStore()
    {
        for(int i = 0 ; i < storeList.Length; i++)
        {
            storeList[i].Unlock(NpcData.Instance.npcArray[i].Unlock);
        }
    }

    public NPC_Talk npcTalk;
}
