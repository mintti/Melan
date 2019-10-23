using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldKnightSelect : MonoBehaviour
{
    public RawImage img;
    public int txrNum;
    public int kNum;

    public void SetData(int num, Texture txr, int _txrNum)
    {
        kNum = num;
        img.texture = txr;
        txrNum = _txrNum;
    } 
}
