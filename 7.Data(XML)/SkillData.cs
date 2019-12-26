using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#region Skill Class Info
public enum SkillType{NONE, COMMON, DAM, SHD, BUF, DBUF, SUPPOSDAM}
public enum Target{NONE, MINE, WE, THAT, THEY} //MINE 우리팀 중 한명
public class SkillInfo
{
    public int job{get;set;}
    public int num{get;set;}
    public string name{get;set;}
    public string explan{get;set;}
    public SkillType type{get;set;}
    public Target target{get;set;}
    public Sprite img{get;set;}

    public SkillInfo(int _job, int _num, Target _target, string _name, string _explan, Sprite _img)
    {
        job = _job;
        num = _num; name = _name; explan = _explan;
        target = _target;
        img = _img;
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
    
    private SkillInfo[,] skillInfos = new SkillInfo[4, 5];
    public SkillInfo GetSkillInfo(int job, int index)
    {
        return skillInfos[job, index];
    }
    #region 스킬정보
    public void SetData()
    {
        string path = "3.Skills/warrior";
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        int i = 0;

        skillInfos[0, 0] = new SkillInfo(0,0, Target.THAT, "타격", "검으로 타격", sprites[i++]);
        skillInfos[0, 1] = new SkillInfo(0, 1, Target.THEY,"휘두르기", "전체 적에게 데미지를 준다.", sprites[i++]);
        skillInfos[0, 2] = new SkillInfo(0, 2, Target.NONE, "방패막기", "다음 공격의 데미지를 낮춘다.", sprites[i++]);
        skillInfos[0, 3] = new SkillInfo(0, 3, Target.NONE, "모으기", "본인의 파워가 3턴간 상승한다.",  sprites[i++]);
        skillInfos[0, 4] = new SkillInfo(0, 4, Target.THAT, "처형", "일정 체력 이하의 적을 즉사시킨다.", sprites[i++]);
        
        path = "3.Skills/wizard_";
        i = 0;
        skillInfos[1, 0] = new SkillInfo(1, 0, Target.THAT, "매직", "마법으로 적을 공격한다.", Resources.Load<Sprite>(path+i++));
        skillInfos[1, 1] = new SkillInfo(1, 1, Target.THEY, "매직샤워", "마법으로 전체 적을 공격한다", Resources.Load<Sprite>(path+i++));
        skillInfos[1, 2] = new SkillInfo(1, 2, Target.NONE, "매직쉴드", "2턴간 받는 데미지를 낮춘다.", Resources.Load<Sprite>(path+i++));
        skillInfos[1, 3] = new SkillInfo(1, 3, Target.THAT, "슬로우", "적의 속도를 늦춘다.", Resources.Load<Sprite>(path+i++));
        skillInfos[1, 4] = new SkillInfo(1, 4, Target.NONE, "?", "?", Resources.Load<Sprite>(path+i));

        path = "3.Skills/Thief";
        sprites = Resources.LoadAll<Sprite>(path);
        i = 0;
        skillInfos[2, 0] = new SkillInfo(2, 0, Target.THAT,"타격", "단검으로 적을 타격", sprites[i++]);
        skillInfos[2, 1] = new SkillInfo(2, 1, Target.MINE, "붕대", "아군의 출혈을 막고, 체력을 회복시킨다", sprites[i++]);
        skillInfos[2, 2] = new SkillInfo(2, 2, Target.NONE, "바람", "본인의 속도를 올린다", sprites[i++]);
        skillInfos[2, 3] = new SkillInfo(2, 3, Target.THEY,  "연막", "전체 적의 시야를 가린다", sprites[i++]);
        skillInfos[2, 4] = new SkillInfo(2, 4, Target.NONE, "도망", "전투에서 도망친다", sprites[i++]);
        
        path = "3.Skills/healer";
        sprites = Resources.LoadAll<Sprite>(path);
        i = 0;
        skillInfos[3, 0] = new SkillInfo(3, 0, Target.THAT, "타격", "적을 공격한다.", sprites[i++]);
        skillInfos[3, 1] = new SkillInfo(3, 1, Target.MINE, "치유", "아군을 체력을 회복시킨다", sprites[i++]);
        skillInfos[3, 2] = new SkillInfo(3, 2, Target.WE, "치유S", "전체 아군을 체력을 회복시킨다.", sprites[i++]);
        skillInfos[3, 3] = new SkillInfo(3, 3, Target.MINE, "정화", "아군의 모든 상태이상을 없앤다", sprites[i++]);
        skillInfos[3, 4] = new SkillInfo(3, 4, Target.MINE, "부활", "죽은 아군을 부활시킨다.", sprites[i++]);
        
    }
    #endregion
    #region 스킬 사용
    public void UseSkill(int job, int num, State playerS, State[] targetsS)
    {
        foreach(State targetS in targetsS)
        {
            switch (job)
            {
                case 0 : //전사
                    switch (num)
                    {
                        case 0 :
                            targetS.AdDam(playerS.Power);
                            break;
                        case 1 :
                            targetS.AdDam((float)playerS.Power * 0.3f);
                            break;
                        case 2 :
                            playerS.AddBuff(BuffType.SHILD, 2, 0.5f);
                            break;
                        case 3 :
                            playerS.AddBuff(BuffType.POWER, 3, 1.5f);
                            break;
                        case 4 :
                            if(targetS.Hp <= targetS.maxHp)
                                targetS.DeathDam();
                            break;
                        default:
                        break;
                    }
                    break;
                case 1 : //법사
                    switch (num)
                    {
                        case 0 :
                            targetS.ApDam(playerS.Power);
                            break;
                        case 1 :
                            targetS.ApDam((float)playerS.Power * 0.3f);
                            break;
                        case 2 :
                            playerS.AddBuff(BuffType.SHILD, 2, 0.7f);
                            break;
                        case 3 :
                            targetS.AddDebuff(BuffType.SPEED, 3, 0.5f);
                            break;
                        case 4 :
                            break;
                        default:
                            break;
                    }
                    break;
                case 2 : //방랑가
                    switch (num)
                    {
                        case 0 :
                            targetS.AdDam(playerS.Power);
                            break;
                        case 1 :
                            targetS.Heal((float)playerS.Power * 0.2f);
                            targetS.Blood = 0;
                            break;
                        case 2 :
                            playerS.AddBuff(BuffType.SPEED, 2, 1.5f);
                            break;
                        case 3 :
                            playerS.Special_Skill_Up++;
                            break;
                        case 4 :
                            playerS.RunBattle();
                            break;
                        default:
                            break;
                    }
                    break;
                case 3 : //힐러
                    switch (num)
                    {
                        case 0 :
                            targetS.AdDam(playerS.Power);
                            break;
                        case 1 :
                            targetS.Heal(playerS.Power);
                            break;
                        case 2 :
                            targetS.Heal((float)playerS.Power * 0.3f);
                            break;
                        case 3 :
                            targetS.Purify();
                            break;
                        case 4 :
                            targetS.Resurrection();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        ColController.Instance.TurnEnd();
    }
    #endregion

    public bool Check(int job, Knight k)
    {
        bool value = false;
        switch (job)
        {
            case 0 :
                break;
            case 1 :
                break;
            case 2 : 
                break;
            case 3 : 
                break;
            default:
                break;
        }

        return value;
    }
}
