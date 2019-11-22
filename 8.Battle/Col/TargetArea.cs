using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetArea : MonoBehaviour
{
    public int targetNum;
    public GameObject select;

    private bool isView;
    public void SetData()
    {
        isView = true;
        ColIsIn(false);
    }
    public void SetData(bool v)
    {   isView = false;
        select.SetActive(false);
    }

    #region 
    private void OnTriggerEnter2D(Collider2D other) {
        ColController.Instance.SetSkillTarget(targetNum);
        ColIsIn(true);
    }
    private void OnTriggerExit2D(Collider2D other) {
        ColController.Instance.SetSkillTarget(-1);
        ColIsIn(false);
    }
    private void ColIsIn(bool v)
    {
        if(!isView) return;
        select.SetActive(v);
    }
    #endregion
}
