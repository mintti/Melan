using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextDay : MonoBehaviour
{   
    public GameObject leftObj;
    public GameObject rightObj;
    
    public Animator animator;

    public void Play()
    {
        SetData(leftObj);
        SetData(rightObj);
        
        animator.Play("NextDay");
    }

    private void SetData(GameObject obj)
    {
        obj.GetComponentInChildren<Text>().text = string.Format("DAY {0}", GameController.Instance.player.Day);
    }
}
