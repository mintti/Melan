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

public enum Target
{
    NONE, ME, S, M
}

public enum ElementType
{
    바람, 물, 대지, 불
}

public class Skill
{
    public int num;
    public string name;
    public string explan;
    public Target target;
    public eType type;
    public int cost;
    public int needP;

    public Skill(int _num, string _name, string _explan, Target _target,
                int _cost, int _needP)
    {
        num = num; name = _name; explan = _explan;
        target = _target; cost = _cost; needP = _needP;
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

    public Thief thief = new Thief();
    public Warrior warrior = new Warrior();
    public Wizard wizard = new Wizard();
    
    public void SetData()
    {

    }

}
//skills[0] = new Skill(0,,,,,,,,);
public class Wizard
{
    public Skill[] skills = new Skill[4];
    
    public Wizard()
    {
        skills[0] = new Skill(0, "속성공격", "한 대상을 시전자의 속성을 담은 마법", Target.S, 2, 1);
        skills[1] = new Skill(1, "속성강화공격", "한 대상을 속성을 2번 담아 사용가능", Target.S, 2, 2);
        skills[2] = new Skill(2, "힐링", "한 명의 아군을 치유",  Target.S, 2, 0);
        skills[3] = new Skill(3, "바인", "한 대상을 덩굴로 적을 속박", Target.S, 2, 0);
    }


    public void Skill(int n, State playerS, State targetS)
    {
        switch(n)
        {
            case 0: //0. 속성공격
                targetS.EleDam(playerS.element.type, playerS.Power);
                playerS.element.Cnt--;
                break;
            case 1: //1. 속성 강화 공격
                targetS.EleDam(playerS.element.type, playerS.Power * 2);
                break;
            case 2: //2. 힐링
                targetS.Heal(System.Convert.ToInt32(playerS.Power * 0.5f));
                break;
            case 3: //3. 바인
                targetS.cc.Bine++;
                break;
            default :
                break;
        }
    }
}

public class Thief
{
    public Skill[] skills = new Skill[4];

    public Thief()
    {
        
        skills[0] = new Skill(0,"단검투척", "한 대상에게 단검을 던짐", Target.S, 1, 0);
        skills[1] = new Skill(1,"약점찾기", "한 대상의 약점을 찾음", Target.S, 3, 0);
        skills[2] = new Skill(2,"암살", "약점 찾기 후 사용가능",Target.S, 3, 0);
        skills[3] = new Skill(3,"탈출", "현재 전투에서 도망친다.", Target.NONE, 3, 0);
    }

    public void Skill(int n, State playerS, State targetS)
    {
        switch(n)
        {
            case 0 :
                break;
            case 1 :
                break;
            case 2 :
                break;
            case 3 :
                break;
            default :
                break;
        }
    }
}

public class Warrior
{
    public Skill[] skills = new Skill[4];

    public Warrior()
    {
        skills[0] = new Skill(0, "베기", "적을 벤다", Target.S, 2, 0);
        skills[1] = new Skill(1, "다중베기", "전체 대상을 벤다",  Target.M, 2, 0);
        skills[2] = new Skill(2, "함성", "적 전체에게 허약을 시도한다", Target.M, 2, 0);
        skills[3] = new Skill(3, "강화", "다음 공격을 강화", Target.ME, 2, 0);
    }

    public void Skill(int n, State playerS, State targetS)
    {
        switch(n)
        {
            case 0 :
                break;
            case 1 :
                break;
            case 2 :
                break;
            case 3 :
                break;
            default :
                break;
        }
    }
}


/* sample
public class Job
{
    public Skill[] skills = new Skill[4];

    public Job()
    {
        skills[0] = new Skill(0, "베기", "적을 벤다", Target.S, 2, 0);
        skills[1] = new Skill(1, "다중베기", "전체 대상을 벤다",  Target.M, 2, 0);
        skills[2] = new Skill(2, "함성", "적 전체에게 허약을 시도한다", Target.M, 2, 0);
        skills[3] = new Skill(3, "강화", "다음 공격을 강화", Target.ME, 2, 0);
    }

    public void Skill(int n, State playerS, State targetS)
    {
        switch(n)
        {
            case 0 :
                break;
            case 1 :
                break;
            case 2 :
                break;
            case 3 :
                break;
            default :
                break;
        }
    }
}

 */