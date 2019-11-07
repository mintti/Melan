using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Form
{
    public enum SkillType{NONE, COMMON, DAM, SHD, BUF, DBUF, SUPPOSDAM}
    public enum DamType{NONE, AD, AP}
    public enum Target{NONE, MINE, WE, THAT, THEY} //MANE 우리팀 중 한명

    public class Skill : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
    {
        private int job;
        private int skillNum;
        
        private string name;
        private string explan;

        public int level;
        public SkillType skillType;
        public DamType damType;
        public Target target;
        public int cost;
        public bool isElement;//ele속성을 사용하는 스킬인가?
        public int cost_Element;

        public Image skillIconImg;
        public Text nameText;
        public Text costText;
        public GameObject cost_ele_Obj; //코스트 부족일때 활성
        public GameObject lockObj;//레벨부족일때 활성

        private bool isLevel; //레벨 부적합.
        private bool isInvisible;

        
        private bool only_View;
        //WizardForm에서 로드케함.
        public void SetData(int _job, int _skillNum, string _name, int _level, SkillType _st, DamType _dt, Target _t, int _cost, bool _isEle, int _cost_ele)
        {
            job = _job; skillNum = _skillNum;

            level = _level; skillType = _st; damType = _dt; target = _t;
            cost = _cost; isElement = _isEle; cost_Element = _cost_ele;

            nameText.text = _name;
            costText.text = cost == 0 ? "" : System.Convert.ToString(cost);
        }
        public void SetData(int _job, Skill _skill)
        {
            job = _job; skillNum = _skill.num;
            name = _skill.name; explan = _skill.explan;

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

        #region 포인터이벤트

        public Timer_Skill timer;

        Vector3 skillPos = new Vector3();
        Vector3 linePos = new Vector3();
        Sprite sprite;

        private bool isPush;
        public bool IsPush{get{return isPush;} 
            set {
                isPush = value;
                if(isPush) ColController.Instance.BeginDrag(transform);
        }}

        public void OnPointerDown(PointerEventData data)
        {
            isPush = false;
            timer.SetData(this, 1f);
            timer.gameObject.SetActive(true);
            
            linePos = transform.GetChild(1).position;
            skillPos = transform.GetChild(2).position;
            sprite = skillIconImg.sprite;
        }
        
        //Timer에서 호출됨.
        public void PushComplete()
        {
            SetIconMod();
            ColController.Instance.SetData(this);
        }
       
        //드래그 이벤트
        public void OnDrag(PointerEventData eventData)
        {
            if(isPush)
                transform.position = eventData.position;
        }
            
        public void OnEndDrag(PointerEventData eventData)
        {
            if(isPush) 
            {
                ColController.Instance.EndDrag(transform);
                SetSkillMod();
            }
        }
        
        //아이콘 <-> 스킬모양
        private void SetIconMod()
        {
            //skillIconImg.sprite = ImageData.Instance.GetSprite(SkillType.NONE);
            skillIconImg.sprite = ColController.Instance.testSkillspr;
            SamMod(true);
        }

        private void SetSkillMod()
        {
            transform.GetChild(1).position = linePos;
            transform.GetChild(2).position = skillPos;
            skillIconImg.sprite = sprite;
            SamMod(false);
        }

        void SamMod(bool isIcon)
        {
            transform.GetChild(1).gameObject.SetActive(!isIcon);
            nameText.enabled = !isIcon;
            costText.enabled = !isIcon;
        }
        #endregion
    }   
}