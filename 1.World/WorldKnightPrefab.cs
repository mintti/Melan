using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldKnightPrefab : MonoBehaviour
{
    #region  Obj Part

    public Image skin;
    public GameObject check;

    #endregion

    public Knight k{get;set;}

    void Start()
    {
        check.SetActive(false);
    }


    public void SetData(Knight _k)
    {
        k = _k;
        skin.sprite = k.skin;
    }

    public void Check()
    {
        check.SetActive(check.activeSelf ? false : true);
    }
}
