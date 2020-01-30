using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour
{
    private bool isTyping;
    public Transform parent;
    public void SetData(Transform p, string text)
    {
        parent = p;
        isTyping = false;
        StartCoroutine("Typing", text);
    }

    public void Click()
    {
        if(isTyping)
        {
            isTyping = false;
        }
        else
        {
            parent.SendMessage("Next");
        }
    }

    IEnumerator Typing(string ment)
    {
        isTyping = true;
        Text text = transform.GetComponent<Text>();

        text.text = null;
        for(int i = 0; i < ment.Length; i++)
        {
            if(!isTyping)
            {
                text.text = ment;
                break;
            }

            text.text += ment[i];
            yield return new WaitForSeconds(0.06f);
        }
        isTyping = false;
    }
}
