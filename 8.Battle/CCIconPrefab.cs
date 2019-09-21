using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCIconPrefab : MonoBehaviour
{
    public Text num;
    public Image icon;

    public void SetData(string type, int num)
    {
        Sprite s = ImageData.Instance.GetSprite(type);
        
        icon.sprite = s;
        this.num.text = string.Format("{0}" , num);
    }
}
