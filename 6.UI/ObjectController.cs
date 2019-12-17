using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private static ObjectController _instance;
    public static ObjectController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(ObjectController)) as ObjectController;

                if(_instance == null)
                {
                    Debug.LogError("There's no active ObjectController object");
                }
            }
            return _instance;
        }
    }


    public GameObject saveObj;

}
