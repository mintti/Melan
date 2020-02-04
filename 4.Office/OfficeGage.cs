using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfficeGage : MonoBehaviour
{
    public RectTransform[] stars = new RectTransform[5];  
    
    public void SetGage()
    {  
        float n = OfficeData.Instance.OfficeGage;
        int i = 0;

        while(i <= 4)
        {
            if(n >= 20 * (i + 1))
                stars[i].sizeDelta = new Vector2(60, 60);
            else
            {
                stars[i].sizeDelta = new Vector2(GetSize(n - (20*i)), 60);
                break;
            }
            i++;
        }
        while(i < 4)
        {
            i++;
            stars[i].sizeDelta = new Vector2(0, 60); 
        } 
    }
    
    private float GetSize(float n)
    {
        if( n == 0 ) return 0;
        return 60 * ( n / 20f);
    }
}
