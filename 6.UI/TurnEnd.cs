using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEnd : MonoBehaviour
{
    Animator animator;
    private bool isDrag;
    private bool touch;

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }
    public void On()
    {
        gameObject.SetActive(true);
        animator.Play("Idle");
        touch = true;
        isDrag = false;
    }
    public void Off()
    {
        gameObject.SetActive(false);
    }
    public void Drag()
    {
        if(!touch) return;

        StartCoroutine("dontTouch");
        if(!isDrag) animator.Play("DragLeft");
        else animator.Play("DragRight");

        isDrag = !isDrag;
    }

    IEnumerator dontTouch()
    {
        touch = false;
        yield return new WaitForSeconds(0.5f);
        touch = true;
    }
}
