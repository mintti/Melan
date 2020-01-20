using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonsterPrefab : MonoBehaviour
{
    public BattleController battle;
    public MonsterState ms;
    public State s;
    public SpriteRenderer img;
    public MonsterStateViewer msv;

    public int index;
    //해당스크립트 활성화 여부임. SetData()를 통해 활성화됨.
    public bool isMonster;
    
    private void Start() {
        battle = BattleController.Instance;
    }

    public void Die()
    {
        msv.gameObject.SetActive(false);
    }
    //BattleCon - MonsterSetting()에서 호출됨.
    public void SetData(Monster m, int n, MonsterStateViewer _msv)
    {
        ms = new MonsterState(m);
        s = ms.s;
        msv = _msv;
        s.SetBMP(this);
        index = n;
        
        isMonster = true;

        TargetOff();
    }

    public void MyTurn()
    {
        ColController.Instance.SetSkillTarget(9);
        gameObject.SendMessage("UseSkill");
        
        battle.NextTurn();
    }
    #region  UI관련
    public GameObject targetObj;
    public void TargetOn()
    {
        targetObj.SetActive(true);
    }
    public void TargetOff()
    {
        targetObj.SetActive(false);

    }
    #endregion

}
