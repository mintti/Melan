using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTextEvent : MonoBehaviour
{
    public Text text;

    public void On()
    {
        gameObject.SetActive(true);

        StartCoroutine("Effect");
    }
    public void Off()
    {
        StopCoroutine("Effect");

        gameObject.SetActive(false);
    }

    IEnumerator Effect()
    {
        int i = 0;
        string[] array = new string[8]{"/", "/\\", "/\\/", "/\\/\\", "/\\/\\/", "/\\/\\/\\", "/\\/\\/\\/", "/\\/\\/\\/\\"};
        while(true)
        {
            text.text = array[i];
            yield return new WaitForSeconds(0.2f);
            i = (++i) % 8;
        }
    }
}
