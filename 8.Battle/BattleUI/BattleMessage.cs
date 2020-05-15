using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMessage : MonoBehaviour
{
    private void Start() {
        gameObject.SetActive(false);
    }
    public void SendMessage(string text)
    {
        gameObject.SetActive(true); 
        GetComponentInChildren<TypingText>().SetData(transform, text);
    }
    public void Next()
    {
        StartCoroutine("Wait");
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
