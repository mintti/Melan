using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public GameObject bubble;
    public Transform bubble_Tr;
    
    public void SendBubble(string ment)
    {
        CodeBox.AddChildInParent(bubble_Tr, bubble).GetComponentInChildren<Bubble>().SetBubble(ment);
    }

    public Transform stateText_Tr;
    public GameObject textObj;
    public void SendHpText(int value)
    {
        CodeBox.AddChildInParent(stateText_Tr, textObj).GetComponentInChildren<StateTextEffect>().SetHpText(value);
    }
    
    public void SendStressText(int value)
    {
        CodeBox.AddChildInParent(stateText_Tr, textObj).GetComponentInChildren<StateTextEffect>().SetStressText(value);
    }
}
