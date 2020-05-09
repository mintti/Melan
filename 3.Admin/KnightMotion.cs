using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMotion : MonoBehaviour
{
    public KnightSkinPrefab skin;
    private Animator animator;

    public bool action;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartAction()
    {
        StartCoroutine("BlinkEyes");
    }
    public void EndAction()
    {
        StopCoroutine("BlinkEyes");
    }
    IEnumerator BlinkEyes()
    {
        while(true)
        {
            float time = Random.Range(3f, 5f);
            yield return new WaitForSeconds(time);
            animator.Play("EyesMotion.CloseEyes");
        }
    }

    public void OrderAction(string name)
    {
        animator.Play(name);
    }
}
