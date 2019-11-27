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
        num = _num; name = _name; explan = _explan;
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

    
    public SkillInfo GetSkillInfo(int job, int num)
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

    public void UseSkill(int job, int num, State playerS, State[] targetsS)
    {
        switch(job)
        {
            case 0 : 
                //warrior.UseSkill(num, playerS, targetsS);
                break;
            case 1 :
                wizard.UseSkill(num, playerS, targetsS);
                break;
            case 2 : 
                //thief.UseSkill(num, playerS, targetsS);
                break;
            default :
                CodeBox.PrintError( string.Format("SkillData - UseSkill() 존재하지 않음, job - {0}, num - {1}", job, num));
                break;
        }
    }
}

//skills[0] = new Skill(0,,,,,,,,);
public class Wizard
{
    public SkillInfo[] skills = new SkillInfo[14];
    
    public Wizard()
    {
        //COMMON
        skills[0] = new SkillInfo(0, "타격", "한 대상을 시전자의 속성을 담은 마법");
        skills[1] = new SkillInfo(1, "방어", "일정량을 방어");
        skills[2] = new SkillInfo(2, "넘기기", "턴 넘기기");
        //SKILL
        skills[3] = new SkillInfo(3, "차칭", "마법 에너지를 모은다");
        skills[4] = new SkillInfo(4, "매직", "한 대상에게 AP공격");
        skills[5] = new SkillInfo(5, "강화매직", "한 대상에게 강화된 AP공격");
        skills[6] = new SkillInfo(6, "매직샤워", "전체 대상에게 AP공격");
        skills[7] = new SkillInfo(7, "강화매직샤워", "전체 대상에게 강화된 AP공격");
        skills[8] = new SkillInfo(8, "매직커터", "AP커팅/절단효과");
        skills[9] = new SkillInfo(9, "잔상", "2턴간 시전자에게 가하는 공격이 일정확률로 빗나간다.");
        skills[10] = new SkillInfo(10, "외곡", "1턴간 아군 전체에게 가하는 공격이 빗나간다.");
        skills[11] = new SkillInfo(11, "슬로우", "한 대상의 SPEED를 낮춘다.");
        skills[12] = new SkillInfo(12, "크러쉬", "한 대상에게 AP데미지를 주고 1턴간 기절시킴. ");
        skills[13] = new SkillInfo(13, "매직쉴드", "2턴간 피해를 감소시킴.");
    }


    public void UseSkill(int n, State playerS, State[] targetsS)
    {
        int ele = 0;
        foreach(State targetS in targetsS)
        {
            switch(n)
            {
                case 0: //타격
                    targetS.AdDam(playerS.Power * 0.2f);
                    break;
                case 1: //방어
                    break;
                case 2: //턴넘기기
                    ColController.Instance.TurnEnd();
                    break;
                case 3: //마나 차칭
                    playerS.element.AddElement(ElementType.M);
                    break;
                case 4: //마나 공격
                    targetS.EleDam(playerS.element.type, playerS.Power);
                    ele = 1;
                    break;
                case 5: //마나 강화공격
                    targetS.EleDam(playerS.element.type, playerS.Power * 2);
                    ele = 2;
                    break;
                case 6: //매직샤워
                    targetS.EleDam(playerS.element.type, playerS.Power * 0.5f);
                    ele =2;
                    break;
                case 7: //강화 매직샤워
                    targetS.EleDam(playerS.element.type, playerS.Power);
                    ele = 3;
                    break;
                case 8: //매직커터
                    targetS.EleDam(playerS.element.type, playerS.Power * 1.5f);
                    //CC
                    ele = 2;
                    break;
                case 9: //잔상
                    ele = 2;
                    break;
                case 10: //외곡
                    ele = 2;
                    break;
                case 11: //슬로우
                    ele = 2;
                    break;
                case 12: //크러쉬
                    ele = 2;
                    break;
                case 13: //매직쉴드
                    ele = 1;
                    break;

                default :
                    break;
            }
        }
        playerS.element.Cnt -= ele;
        ColController.Instance.UpdateData();
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

    public void UseSkill(int n, State playerS, State targetS)
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

    public void UseSkill(int n, State playerS, State targetS)
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