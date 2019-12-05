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
            if(hp> maxHp)
                hp = maxHp;
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
    public int gage;
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
        Power = ks.k.power;
        Speed = ks.k.speed;
        Stress = ks.k.stress;
        uni = ks.k.uni;

        LifeType = LifeType.K;

        impact = new Del(선언);

        powerMultiple = 1;
        speedMultiple = 1;
    }
    
    //몬스터
    public void SetData(MonsterState ms)
    {
        hp = ms.m.hp;
        Power = ms.m.power;
        Speed = ms.m.speed;
        Stress = ms.m.stress;
        uni = ms.m.uni;

        LifeType = LifeType.M;

        impact = new Del(선언);

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
            BattleController.Instance.DieKnight(bkp.index);
        }
        else
        {
            bmp.Die();
            BattleController.Instance.KillMonster(bmp.index);
        }
    }
    public void NextTurn()
    {

    }
    #region 상태이상 효과 관련
    private Del impact;

    private bool changeImpact;//변경되면 재계산해야함. 추후 설정되게 해주어야함.

    public AfterDil afterDil = new AfterDil();
    private bool pass; //스턴같은 경우.

    public class AfterDil
    {
        public int pois{get;set;}
        public int blo{get;set;}

        public AfterDil()
        {
            pois = 0;
            blo = 0;
        }
        public int Sum()
        {
            return pois + blo;
        }
    }

    /*
        Impact()는 Player 턴을 넘길경우 false를 보낸다.
                         */
    public bool Impact(bool type) //type = false : 시작, true : 종료
    {
        impact();
        //if(changeImpact){} *이 안에 impact()가 오게..
        //스턴 등 의 여부.
        if(pass) return false;
        return true;
    }
    public void 선언()
    {
        powerMultiple = 1;
        speedMultiple = 1;
        changeImpact = false;
        afterDil = new AfterDil();
        pass = false;
    }
    //좋은 영향
    public void 강화()
    {   
        powerMultiple *=2;
    }
    //안좋은영향
    public void 스트레스(int v)
    {
        stress += v;
    }

    
    public CC cc = new CC();
    public SuperCC superCc = new SuperCC();
    //후딜
    public void 출혈()
    {   afterDil.blo++; }
    public void 중독()
    {   afterDil.pois++; }
    
    #endregion


    //공격 받음
    public void AdDam(float v)
    {
        Hp -= System.Convert.ToInt32(v);
    }
    public void ApDam(float v)
    {
        Hp -= System.Convert.ToInt32(v);
    }

    public void Heal(float v)
    {
        Hp += System.Convert.ToInt32(v);
    }

    //즉사

    public void DeathDam()
    {
        Hp = 0;
    }


    #region 속성공격
    public void EleDam(float v)
    {
        Hp -= System.Convert.ToInt32(v);
    }

    #endregion

}
