using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_Type_Monster_Motion : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine("BlinkEyes");
    }

    public void Action(string action)
    {
        animator.Play(action);
    }
    IEnumerator BlinkEyes()
    {
        while(true)
        {
            float time = Random.Range(5f, 10f);
            yield return new WaitForSeconds(time);
            animator.Play("EyesMotion.CloseEyes");
        }
    }
}
