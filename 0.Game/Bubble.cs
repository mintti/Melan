using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Bubble : MonoBehaviour
{
    public void SetBubble(string _text)
    {
        gameObject.SetActive(true);
        GetComponentInChildren<Text>().text =_text;

        StartCoroutine("Close");
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    
}
