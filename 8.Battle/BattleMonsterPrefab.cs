﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonsterPrefab : MonoBehaviour
{
    public MonsterState ms;
    public State s;
    public SpriteRenderer img;
    public SpriteRenderer shadow;

    //해당스크립트 활성화 여부임. SetData()를 통해 활성화됨.
    public bool isMonster;
    
    private void Start() {
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
        shadow.sprite = m.img;

        isMonster = true;
    }

    public int GetSingleTarget()
    {
        int target = Random.Range(0, BattleController.Instance.knightTarget.Count);

        return target;
    }
}
