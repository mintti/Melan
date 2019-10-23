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
    public Image employImg;

    private Button bt;
    private bool isEmploy;

    public void SetData(RandomKnight rk, int _n)
    {
        n = _n;
        knight = rk;

        nameText.text = knight.name;
        jobText.text = string.Format("{0}", TextData.Instance.job_lan[knight.job]);
        levelText.text = string.Format("{0}급" , knight.level);

        bt = GetComponent<Button>();

        isEmploy = false;
        
        if(knight.isEmploy == true)
            Employment();
        else
            NonCheck();
    }
    public void NonCheck()
    {
        if(isEmploy == true)   return;   

        SetColor(new Color(150, 150, 150));
        line.enabled = false;
        employImg.enabled = false;
        
        bt.interactable = true;
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
    }

    //고용되서 더이상 선택불가.
    public void Employment()
    {
        
        NonCheck();
        isEmploy = true;
        employImg.enabled = true;
        bt.interactable = false;

    }
}
