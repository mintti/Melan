using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMotion : MonoBehaviour
{
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OrderMotion(string name)
    {
        animator.Play(name);
    }
}
