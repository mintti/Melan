﻿using System.Collections;
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
    public int Stress{get{return stress;} set{stress = value;}}
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
        Stress = ks.k.stress;
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
        Stress = ms.m.stress;
        uni = ms.m.uni;

        LifeType = LifeType.M;

        powerMultiple = 1;
        speedMultiple = 1;
    }
    public void SetBMP(BattleMonsterPrefab bmp_)
    {
        bmp = bmp_;
    }
    public void SetBKP(BattleKnightPrefab bkp_)
    {
        bkp = bkp_;
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
        alive = AliveType.죽음;
        if(lifeType == LifeType.K)
        {
            //BattleController.Instance.DieKnight(bkp.index);
        }
        else
        {
            bmp.Die();
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

    private void PoisonDam()
    {
        
    }
    private void BloodDam()
    {
    }
    public void Purify()
    {
        debuffs.Clear();
    }
    //공격 받음
    public void AdDam(float v)
    {
        Hp -= (int)v;
    }
    public void ApDam(float v)
    {
        Hp -= System.Convert.ToInt32(v);
    }

    public void Heal(float v)
    {
        Hp += (int)v;
    }

    public void EleDam(float v)
    {
        Hp -= System.Convert.ToInt32(v);
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
}
