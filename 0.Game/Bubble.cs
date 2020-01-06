using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Bubble : MonoBehaviour
{
    public GameObject targetObj;

    public void SetBubble(string _text)
    {
        targetObj.SetActive(true);
        GetComponent<Text>().text =_text;
        
        Vector2 vc = GetComponent<RectTransform>().sizeDelta;
        if(vc.y < 120f) vc.y = 120f; else vc.y += 90;
        targetObj.GetComponent<RectTransform>().sizeDelta = vc;

        StartCoroutine("Close");
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(1f);
        targetObj.SetActive(false);
    }
    
}
