using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//생존 시 상태 여부
public enum AliveType
{
    생존,
    죽음
}
public enum LifeType
{
    K, M
}
public enum ElementType
{
    M
}

public class CC
{
    //기절 > 속박 > 허약 = 약점
    private int weakness;//허약
    public int Weakness{get{return weakness;} set{weakness = value; if(weakness < 0) weakness = 0;}}
    private int weakPoint;//약점
    public int WeakPoint{get{return weakPoint;} set{weakPoint = value; if(weakPoint < 0) weakPoint = 0;}}
    private int stun;//기절
    public int Stun{get{return stun;} set{stun = value;}}
    private int bine; //속박
    public int Bine{get{return bine;} set{bine = value; if(bine < 0) bine = 0;}}

    public CC()
    {
        Stun = 0;
        WeakPoint = 0;
        Weakness = 0;
        Bine = 0;
    }

    public void Clear()
    {
        Weakness = 0;
        WeakPoint = 0;
        Stun = 0;
        Bine = 0;
    }

    public void UpdateCC()
    {
        Stun = 0;
        Weakness--;
        WeakPoint--;
        Bine--;
    }
}
//스킬에 관련된 상태이상들.
public class SuperCC
{
    private bool weakPoint;
    public bool WeakPoint{get{return weakPoint;} set{weakPoint = value;}}
    private int taunt;
    public int Taunt{get{return taunt;}set{taunt = value;}}
    public SuperCC()
    {
        weakPoint = false;
    }
}

public enum BuffType
{
    POWER, SPEED, SHILD
}
class Buff
{
    public int turn{get;set;}
    public float factor{get;set;}
    BuffType type{get;set;}
    public Buff(BuffType _type, int _turn, float _factor)
    {
        turn = _turn; factor = _factor; type = _type;
    }

    public void NextTurn()
    {
        turn --;
    }
}

[System.Serializable]
public class State
{
    BattleKnightPrefab bkp;
    BattleMonsterPrefab bmp;
    BubbleController bubble;

    delegate void Del();

    //기본 변수
    private int hp;
    public int Hp{get{return hp;}
        set{
            hp = value;
            if(hp > maxHp)
            {
                hp = maxHp; 
            }
            else if(hp<=0)
            {
                hp =0;
                Die();
                return;
            }
            UpdateDataUI();
        }
    }
    public int maxHp;
    private int power;
    public int Power{get{int value = System.Convert.ToInt32(power* powerMultiple); return value;} set{power = value;}}
    private int speed;
    public int Speed{get{int value = System.Convert.ToInt32(speed* speedMultiple); return value;} set{speed = value;}}
    private int stress;
    public int Stress{get{return stress;}
        set{
            stress = value;
            if(stress > 100)
            {
                stress = 100; 
            }
            else if(stress<0)
            {
                stress =0;
            }
            UpdateDataUI();
        }}
    public int[] uni;

    //배수 변수
    private float powerMultiple;
    private float speedMultiple;

    public AliveType alive;
    //이건 몬스터인가?
    private LifeType lifeType;
    public LifeType LifeType{get{return lifeType;} set{lifeType = value;}}

    #region 초기세팅
    public void SetData(KnightState ks)
    {
        hp = ks.k.hp;
        maxHp = ks.k.maxHp;
        Power = ks.k.power;
        Speed = ks.k.speed;
        stress = ks.k.stress;
        uni = ks.k.uni;

        LifeType = LifeType.K;

        powerMultiple = 1;
        speedMultiple = 1;
    }
    
    //몬스터
    public void SetData(MonsterState ms)
    {
        hp = ms.m.hp;
        maxHp = hp;
        Power = ms.m.power;
        Speed = ms.m.speed;
        stress = ms.m.stress;
        uni = ms.m.uni;

        LifeType = LifeType.M;

        powerMultiple = 1;
        speedMultiple = 1;
    }
    public void SetBMP(BattleMonsterPrefab bmp_)
    {
        bmp = bmp_;
        bubble = bmp.msv.transform.GetComponent<BubbleController>();
    }
    public void SetBKP(BattleKnightPrefab bkp_)
    {
        bkp = bkp_;
        bubble = bkp.transform.GetComponent<BubbleController>();
    }

    #endregion

    #region  게임진행관련
    private void UpdateDataUI()
    {
        if(lifeType == LifeType.K)
        {
            bkp.UpdateText();
        }
        else
        {
            bmp.msv.UpdateData();
        }
    }
    //사망처리
    public void Die()
    {
        if(alive == AliveType.죽음) return;

        alive = AliveType.죽음;
        if(lifeType == LifeType.K)
        {
            //BattleController.Instance.DieKnight(bkp.index);
        }
        else
        {
            bmp.StartCoroutine("Die");
            BattleController.Instance.KillMonster(bmp.index);
        }
    }
    public void NextTurn()
    {
        foreach(Buff buff in buffs)
            buff.NextTurn();
    }
    public void MyTurn()
    {
        if(stun);//턴넘김
        
    }
    #endregion
    
    #region UI관련
    private void Dam(float value)
    {
        if(alive == AliveType.죽음) return;

        Hp += (int)value;
        bubble.SendHpText((int)value);

        Motion_Dam();
    }

    private void SetStress(int value)
    {
        if(alive == AliveType.죽음) return;

        Stress += (int)value;
        bubble.SendStressText((int)value);
    }

    #endregion
    
    #region 스킬 사용 관련
 
    bool stun;
    int slow;
    private int blood;
    public int Blood{get{return blood;}set{blood = value;}}
    private int poison;
    private int special_Skill_Up;
    public int Special_Skill_Up{get{return special_Skill_Up;}set{special_Skill_Up = value;}}

    private List<Buff> buffs = new List<Buff>();
    private List<Buff> debuffs = new List<Buff>();
    
    public void AddBuff(BuffType type, int turn, float factor)
    {   
        buffs.Add(new Buff(type, turn, factor));
    }

    public void AddDebuff(BuffType type, int turn, float factor)
    {   
        debuffs.Add(new Buff(type, turn, factor));
    }

    private void AddPoison(int cnt)
    {
                
    }
    private void AddBlood(int cnt)
    {
        Blood += cnt;
    }
    public void Purify()
    {
        debuffs.Clear();
    }
    //공격 받음
    public void AdDam(float v)
    {
        Dam(-v);
    }
    public void ApDam(float v)
    {
        Dam(-v);
    }

    public void Heal(float v)
    {
        Dam(v);
    }

    public void EleDam(float v)
    {
        Dam(-v);
    }

    
    //즉사

    public void DeathDam()
    {
        Hp = 0;
    }

    public void RunBattle()
    {

    }

    public void Resurrection()
    {

    }

    
    #endregion
    #region 모션

    public void Motion(string command)
    {
        if(lifeType == LifeType.K)
        {
            bkp.Motion(command);
        }
        else
        {
            bmp.Motion(command);
        }
    } 

    public void Motion_Dam()
    {
        Motion("Dam");
        Motion("Tr.MyTurn");
    }

    #endregion
}
