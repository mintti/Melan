using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StateTextEffect : MonoBehaviour
{
    private Color32 damageColor = new Color32(210, 0, 0, 255);
    private Color32 healColor = new Color32(38, 255, 126, 255);
    private Color32 stressColor = new Color32(0, 0, 0, 255);
    private Color32 unstressColor = new Color32(255, 255, 255, 255);

    public Text text;

    private int n;
    public void SetHpText(int _n)
    {
        n = _n;
        text.color =  n < 0 ? damageColor : healColor;

        SetText(false);
    }
    public void SetStressText(int _n)
    {
        n = _n;
        text.color = n < 0 ? unstressColor : stressColor ;

        SetText(true);
    }

    private void SetText(bool per)
    {
        text.text = string.Format("{0}{1}{2}", n < 0 ? "" : "+" , n, per ? "%" : " ");

        StartCoroutine("Effect");
        Destroy(gameObject, 1);
    }

    IEnumerator Effect()
    {
        while(true)
        {
            transform.position += new Vector3(0, 1, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
