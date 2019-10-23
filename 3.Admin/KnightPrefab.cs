using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightPrefab : MonoBehaviour
{
    #region  Obj Part

    public Image jobImg;
    public Text name;
    public Text stress;
    
    #endregion

    public Knight k{get;set;}

    public void SetData(Knight _k)
    {
        k = _k;
        jobImg.sprite = ImageData.Instance.job[k.job];
        name.text = string.Format("{0}", k.name);
        stress.text = k.stress + "%";      
    }

    
}
