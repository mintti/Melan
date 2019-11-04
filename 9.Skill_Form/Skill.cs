using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Form
{
    public enum SkillType{NONE, COMMON, DAM, SHD, BUF, DBUF, SUPPOSDAM}
    public enum DamType{NONE, AD, AP}
    public enum Target{NONE, ME, MINE, WE, THAT, THEY} //MANE 우리팀 중 한명

    public class Skill : MonoBehaviour
    {
        public int level;
        public SkillType skillType;
        public DamType damType;
        public Target target;
        public int cost;
        public bool isElement;//ele속성을 사용하는 스킬인가?
        public int cost_Element;

        public Text nameText;
        public Text costText;
        public GameObject cost_ele_Obj; //코스트 부족일때 활성
        public GameObject lockObj;//레벨부족일때 활성

        private bool isLevel; //레벨 부적합.
        
        //WizardForm에서 로드케함.
        public void SetData(string _name, int _level, SkillType _st, DamType _dt, Target _t, int _cost, bool _isEle, int _cost_ele)
        {
            level = _level; skillType = _st; damType = _dt; target = _t;
            cost = _cost; isElement = _isEle; cost_Element = _cost_ele;

            nameText.text = _name;
            costText.text = cost == 0 ? "" : System.Convert.ToString(cost);
        }


        //Lock의 여부
        public void SetLock(int knightLevel)
        {
            Lock(knightLevel >= level ? false : true);
        }
        public void SetLock(bool value)
        {
            Lock(value);
        }

        //버튼의 활성비활성
        public void UpdateUserCost_Element(int cost)
        {
            if(!isLevel || !isElement) return;
           
            Active( cost >= cost_Element ? true : false);
        }
        

        //레벨딸려 비활 || 적의 공격으로 비활
        private void Lock(bool value)
        {
            lockObj.SetActive(value);
            costText.gameObject.SetActive(!value);
            isLevel = !value;
        }
        
        //Ele사용 스킬 cost충분 여부.
        private void Active(bool value)
        {
            cost_ele_Obj.SetActive(value);
            costText.gameObject.SetActive(value);
        }
    }   
}