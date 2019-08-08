using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Skill Class Info
public enum eType
{
    AD,
    AP,
    B,
    DB,
    미정
}

public enum SkillImpact
{
    NONE,
    출혈,
    스트레스,
    스턴,
    허약,
    약점
}

[System.Serializable]
public class Skill
{
    public int num;
    /* XMl에서 호출 */
    public string name{get;set;}
    public string explan{get;set;}

    /* 함수 내에서 지정. */
    public eType type{get;set;} //스킬 타입

    public int targetCnt{get;set;} //공격 적의 수
    public float multiple{get;set;} //power x multiple
    public SkillImpact impact{get;set;}
    public int pro{get;set;} //임팩트 확률
    
    
    public Skill(string _name, string _explan, eType _type,
        int _targetCnt, float _multiple, SkillImpact _impact, int _pro)
    {
        name = _name;
        explan = _explan;
        type = _type;
        targetCnt = _targetCnt;
        multiple = _multiple;

    }
}
#endregion

public class SkillData : MonoBehaviour
{

    private static SkillData _instance = null;

    public static SkillData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(SkillData)) as SkillData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active SkillData object");
                }
            }
            return _instance;
        }
    }

    public Skill[] skills = new Skill[10];


    public void Test_InsertData(){

        /*
        //0. 미정
        skills[0] = new Skill("미정", "설명", eType.미정, 1,
                              new float[]{0.1f, 0.1f, 0.1f, 0.1f, 0.1f});

        //1. 공격.                   
        skills[1] = new Skill("공격", "설명", eType.AD, 1,
                              new float[]{0.1f, 0.1f, 0.1f, 0.1f, 0.1f});

        //2. 마법. 
        skills[2] = new Skill("마법", "설명", eType.AP, 1,
                              new float[]{0.1f, 0.1f, 0.1f, 0.1f, 0.1f});

        //3. 버프. 
        skills[3] = new Skill("버프", "설명", eType.B, 1,
                              new float[]{0.1f, 0.1f, 0.1f, 0.1f, 0.1f});

        //4. 디퍼브. 
        skills[4] = new Skill("디버프", "설명", eType.DB, 1,
                              new float[]{0.1f, 0.1f, 0.1f, 0.1f, 0.1f});

         */

    }
    
}
