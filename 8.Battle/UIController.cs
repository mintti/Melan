using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private void Start() {
        ClickMiddleButton(1);
    }
    #region Middle
    public Image gameImg;
    public Image textImg;
    public Image skillImg;

    public GameObject gameInfoObj;
    public GameObject talkingUIObj;

    public void ClickMiddleButton(int n)
    {
        gameImg.color = n == 0 ? new Color(255, 255, 255) :new Color(0, 0, 0);
        textImg.color = n == 1 ? new Color(255, 255, 255) :new Color(0, 0, 0);
        skillImg.color = n == 2 ? new Color(255, 255, 255) :new Color(0, 0, 0);

        gameInfoObj.SetActive(n == 0);
        talkingUIObj.SetActive(n == 1);
    }
    #endregion
}
