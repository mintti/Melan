using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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


    //string -> int[] 로 반환. ex) "51245" -> int[]{5, 1, 2, 4, 5}
    public static int[] StringAsIntArr(string s)
    {
        int n = System.Convert.ToInt32(s);
        int size = s.Length;
        int[] arr = new int[size];

        for(int i = 1; n>0; n/=10)
        {
            arr[size - i++] = n%10;
        }

        return arr;
    }

    public static int[] StringSplit(string s)
    {
        string[] sArr = s.Split(',');
        int[] Arr = new int[sArr.Length];

        for(int i = 0; i< sArr.Length; i++)
        {
            Arr[i] = System.Convert.ToInt32(sArr[i]);
        }

        return Arr;
    }

    public static void PrintError(string msg)
    {
        GameObject msgObj = Resources.Load("Prefabs/msgObj") as GameObject;
        msgObj.transform.GetComponentInChildren<Text>().text = msg;
        Instantiate(msgObj, new Vector3(0, 0, 0), Quaternion.identity);

        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        msgObj.transform.SetParent(canvas.transform, false);
    }
}
