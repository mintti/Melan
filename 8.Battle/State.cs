using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//생존 시 상태 여부
public enum AliveType
{
    생존,
    죽음
}
public class State : MonoBehaviour
{
    delegate void Del();

    //기본 변수
    public int hp;
    private int power;
    public int Power{get{int value = System.Convert.ToInt32(power* powerMultiple); return value;} set{power = value;}}
    private int speed;
    public int Speed{get{int value = System.Convert.ToInt32(speed* speedMultiple); return value;} set{speed = value;}}
    public int stress;
    public List<int> uni = new List<int>();
    //배수 변수
    private float powerMultiple;
    private float speedMultiple;

    public State(int h, int p, int s, int st, List<int> u)
    {
        hp = h; Power = p; Speed = s; stress = st;
        uni = u;

        impact = new Del(선언);
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
    public void 허약()
    {}
    public void 약점()
    {}
    public void 실명()
    {}
    //후딜
    public void 출혈()
    {   afterDil.blo++; }
    public void 중독()
    {   afterDil.pois++; }
    //상태영향
    public void 스턴()
    {

    }
    
    
    public void 속박()
    {}
    #endregion
}
