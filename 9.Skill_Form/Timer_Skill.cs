using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Form;

public class Timer_Skill : MonoBehaviour
{
    private float goalTime;    
    private float time;

    private Form.Skill skill;

    public void SetData(Form.Skill _skill, float _goalTime)
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
}