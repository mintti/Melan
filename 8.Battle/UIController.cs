using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private void Start() {
        ClickTextButton();
    }
    #region Middle
    public Image textImg;
    public Image skillImg;

    public GameObject talkingUIObj;

    public void ClickTextButton()
    {
        skillImg.color = new Color(0, 0, 0);
        textImg.color = new Color(255, 255, 255);

        talkingUIObj.SetActive(true);
    }
    public void ClickSkillButton()
    {
        skillImg.color = new Color(255, 255, 255);
        textImg.color = new Color(0, 0, 0);

        talkingUIObj.SetActive(false);
    }
    #endregion
}
