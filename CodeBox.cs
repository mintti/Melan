using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CodeBox : MonoBehaviour
{
    //Object 자식 제거 함수
    public static void ClearList(Transform list)
    {
        foreach(Transform t in list)
        {
            Destroy(t.gameObject);
        }
    }

    public static GameObject AddChildInParent(Transform parent, GameObject child)
    {
        GameObject obj = Instantiate(child);
        obj.transform.SetParent(parent, false); //부모-자식 지정.

        return obj;
    }

    //2차원 배열 크기 변경
    public static Array ResizeArray(Array arr, int[] newSizes)
    {
        if (newSizes.Length != arr.Rank)
        throw new ArgumentException("arr must have the same number of dimensions " +
                                     "as there are elements in newSizes", "newSizes"); 

        var temp = Array.CreateInstance(arr.GetType().GetElementType(), newSizes);
        int length = arr.Length <= temp.Length ? arr.Length : temp.Length;
        Array.ConstrainedCopy(arr, 0, temp, 0, length);
        
        return temp;
   }   


}
