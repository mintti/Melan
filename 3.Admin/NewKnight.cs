using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewKnight : MonoBehaviour
{
    public AdminNewKnight adminNewKnight;

    private RandomKnight knight;
    private int n;

    public Text nameText;
    public Text jobText;
    public Text levelText;
    
    public RawImage image;
    public Outline line;

    public void SetData(RandomKnight nk, int _n)
    {
        n = _n;
        knight = nk;

        nameText.text = knight.name;
        jobText.text = string.Format("{0}", TextData.Instance.job_lan[knight.job]);
        levelText.text = string.Format("{0}급" , knight.level);

        NonCheck();
    }
    public void NonCheck()
    {
        SetColor(new Color(150, 150, 150));
        line.enabled = false;
    }
    public void Check()
    {
        SetColor(new Color(255, 255, 255));
        line.enabled = true;
    }

    private void SetColor(Color c)
    {
        nameText.color = c;
        jobText.color = c;
        levelText.color = c;
        image.color = c;
    } 

    public void Click()
    {
        adminNewKnight.ClickSignal(n);
        Debug.Log("sig : " + n);
    }
}
