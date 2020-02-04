using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeData : MonoBehaviour
{
    private static OfficeData _instance;
    public static OfficeData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(OfficeData)) as OfficeData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active OfficeData object");
                }
            }
            return _instance;
        }
    }

    private float officeGage;
    public float OfficeGage{get{return officeGage;}
        set{
            officeGage = value;
            if(officeGage < 0) officeGage = 0;
            else if(officeGage >100)
                {
                    officePoint +=1;
                    officeGage %= 100;
                }
            }}    
    public int officePoint;
}
