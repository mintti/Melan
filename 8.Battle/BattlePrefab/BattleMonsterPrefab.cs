using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMonsterPrefab : MonoBehaviour
{
    public MonsterState ms;
    public State s;
    public SpriteRenderer img;
    public MonsterStateViewer msv;
    public MonsterMotion motion;

    public int index;
    //해당스크립트 활성화 여부임. SetData()를 통해 활성화됨.
    public bool isMonster;

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.5f);

        msv.gameObject.SetActive(false);
        gameObject.SetActive(false);

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
        gameObject.SetActive(true);
        msv.gameObject.SetActive(true);

        TargetOff();
    }

    public void MyTurn()
    {
        ColController.Instance.SetSkillTarget(9);
        gameObject.SendMessage("UseSkill");
        
        BattleController.Instance.StartCoroutine("NextTurn");
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


    #region MOTION
    private Delegate motions;
    public void Motion(string command)
    {
        motion.OrderMotion(command);
    }

    #endregion

}
