using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Bubble : MonoBehaviour
{
    float time;
    float[] time_Array = new float[2]{1, 2};

    public void SetBubble(string _text)
    {
        gameObject.SetActive(true);
        GetComponentInChildren<Text>().text =_text;

        time = _text.Length < 20 ? time_Array[0] : time_Array[1];
        
        Destroy(gameObject, time);
        //StartCoroutine("Close");
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    
}
