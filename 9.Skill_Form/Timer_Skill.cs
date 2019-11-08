using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer_Skill : MonoBehaviour
{
    private float goalTime;    
    private float time;

    private Skill skill;

    public void SetData(Skill _skill, float _goalTime)
    {
        skill = _skill;
        goalTime = _goalTime;
        time = 0;
    }

    private void Update () {
        time += Time.deltaTime;
        if(goalTime < time)
        {
            skill.IsPush = true;
            skill.PushComplete();
            gameObject.SetActive(false);
        }
    }

    public void TimerEnd()
    {
        gameObject.SetActive(false);
    }
}