using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Skill Class Info

public class SkillInfo
{
    public int num;
    public string name;
    public string explan;

    public SkillInfo(int _num, string _name, string _explan)
    {
        num = num; name = _name; explan = _explan;
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


    public SkillInfo GetSkill(int job, int num)
    {
        switch(job)
        {
            case 0 : 
                return warrior.skills[num];
            case 1 :
                return wizard.skills[num];
            case 2 : 
                return thief.skills[num];
            default :
                CodeBox.PrintError( string.Format("SkillData - GetSkill() 존재하지 않음, job - {0}, num - {1}", job, num));
                return null;
        }
    }
}

//skills[0] = new Skill(0,,,,,,,,);
public class Wizard
{
    public SkillInfo[] skills = new SkillInfo[4];
    
    public Wizard()
    {
        skills[0] = new SkillInfo(0, "속성공격", "한 대상을 시전자의 속성을 담은 마법");
        skills[1] = new SkillInfo(1, "속성강화공격", "한 대상을 속성을 2번 담아 사용가능");
        skills[2] = new SkillInfo(2, "힐링", "한 명의 아군을 치유");
        skills[3] = new SkillInfo(3, "바인", "한 대상을 덩굴로 적을 속박");
    }


    public void SkillInfo(int n, State playerS, State targetS)
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
                targetS.Heal(playerS.Power * 0.5f);
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
    public SkillInfo[] skills = new SkillInfo[4];

    public Thief()
    {
        
        skills[0] = new SkillInfo(0,"단검투척", "한 대상에게 단검을 던짐");
        skills[1] = new SkillInfo(1,"약점찾기", "한 대상의 약점을 찾음");
        skills[2] = new SkillInfo(2,"암살", "약점 찾기 후 사용가능");
        skills[3] = new SkillInfo(3,"탈출", "현재 전투에서 도망친다.");
    }

    public void SkillInfo(int n, State playerS, State targetS)
    {
        switch(n)
        {
            case 0 ://단검투척
                targetS.AdDam(playerS.Power * 0.5f);
                break;
            case 1 ://약점찾기
                
                break;
            case 2 ://암살
                if(targetS.superCc.WeakPoint == true)
                    targetS.DeathDam();
                break;
            case 3 ://탈출
                
                break;
            default :
                break;
        }
    }
}

public class Warrior
{
    public SkillInfo[] skills = new SkillInfo[4];

    public Warrior()
    {
        skills[0] = new SkillInfo(0, "베기", "적을 벤다");
        skills[1] = new SkillInfo(1, "다중베기", "전체 대상을 벤다");
        skills[2] = new SkillInfo(2, "함성", "적 전체에게 허약을 시도한다");
        skills[3] = new SkillInfo(3, "강화", "다음 공격을 강화");
    }

    public void Skill(int n, State playerS, State targetS)
    {
        switch(n)
        {
            case 0 ://베기
            targetS.AdDam(playerS.Power);
                break;
            case 3 ://강화
                break;
            default :
                break;
        }
    }
    public void MultiSkill(int n, State playerS, State[] targetS)
    {
        switch(n)
        {
            case 1 : //다중베기
                foreach (State s in targetS)  s.AdDam(playerS.Power * 0.3f);
                break;
             case 2 ://허약
                break;
            default :
                break;
        }
    }
}