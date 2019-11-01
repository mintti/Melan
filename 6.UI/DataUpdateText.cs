using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataUpdateText : MonoBehaviour
{
    public Text text;
    
    private void Start() {
        Destroy(this.gameObject, 2);
    }
    public void TextUpdate(string _text)
    {
        this.gameObject.SetActive(true);
        text.text = _text;
        //StartCoroutine(timer());    
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
    }

}
