using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonsterPrefab : MonoBehaviour
{
    public BattleController battle;
    public MonsterState ms;
    public State s;
    public SpriteRenderer img;
    

    //해당스크립트 활성화 여부임. SetData()를 통해 활성화됨.
    public bool isMonster;
    
    private void Start() {
        battle = BattleController.Instance;
        isMonster = false;
    }

    public void Die()
    {
        isMonster = false;
    }
    //BattleCon - MonsterSetting()에서 호출됨.
    public void SetData(Monster m)
    {
        ms = new MonsterState(m);
        s = ms.s;
        
        img.sprite = m.img;
        isMonster = true;

         TargetOff();
    }

    public void MyTurn()
    {

    }

    public GameObject targetObj;
    public void TargetOn()
    {
        targetObj.SetActive(true);
    }
    public void TargetOff()
    {
        targetObj.SetActive(false);

    }

    public State GetSingleTarget()
    {
        int targetNum = Random.Range(0, battle.knightTarget.Count);
        State target = battle.thing[targetNum];
        
        return target;
    }

    public State GetSingleTarget_Lowest_HP()
    {
        int targetNum = 0;
        for(int i = 1 ; i < battle.knightTarget.Count; i++)
        {
           targetNum = battle.thing[targetNum].Hp < battle.thing[i].Hp ? targetNum : i;
        }
        State target = battle.thing[targetNum];
        return target;
    }
}
