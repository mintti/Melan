﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

    public enum SkillType{NONE, COMMON, DAM, SHD, BUF, DBUF, SUPPOSDAM}
    public enum DamType{NONE, AD, AP}
    public enum Target{NONE, MINE, WE, THAT, THEY} //MANE 우리팀 중 한명

public class Skill : MonoBehaviour, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{   
    private KnightState ks;
    public int job{get;set;}
    public int skillNum{get;set;}
        
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
    public GameObject eleCost;
    public GameObject cost_ele_Obj; //코스트 부족일때 활성
    public GameObject lockObj;//레벨부족일때 활성

    private bool isLevel; //레벨 부적합.
    private bool isInvisible;

    int knightCost;
    int knightEleCost;

        
    private bool only_View;
    
    public void SetData(KnightState _ks, SkillInfo skillInfo)
    {
        //기본정보
        ks = _ks;
        job = ks.k.job;
        SetLock(ks.k.level);

        //스킬정보
        skillNum = skillInfo.num;
        name = skillInfo.name; explan = skillInfo.explan;

        nameText.text = name;
        costText.text = cost == 0 ? "" : System.Convert.ToString(cost);
        if(isElement)
        {
            eleCost.SetActive(true);
            eleCost.GetComponentInChildren<Text>().text = System.Convert.ToString(cost_Element);
        }
        else eleCost.SetActive(false);

        knightCost = ColController.Instance.formObj[ColController.Instance.who].GetComponent<Form>().Cost;
        knightEleCost = ks.s.element.Cnt;
        UpdateCost();
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
    
    //업데이트//버튼의 활성비활성
    public void UpdateCost()
    {
        CheckCost();
        CheckEleCost();
    }
    //그냥 코스트
    void CheckCost()
    {
        if(!isLevel || !isElement) return;
        
        Active(  knightCost >= cost_Element ? true : false);
    }
    //엘레
    public void CheckEleCost()
    {
        if(!isLevel || !isElement) return;
        
        Active( knightEleCost >= cost_Element ? true : false);
    }
        

    //레벨딸려 비활 || 적의 공격으로 비활
    private void Lock(bool value)
    {
        lockObj.SetActive(value);
        costText.gameObject.SetActive(!value);
        nameText.gameObject.SetActive(!value);
        isLevel = !value;
    }
        
        //Ele사용 스킬 cost충분 여부.
        private void Active(bool value)
        {
            cost_ele_Obj.SetActive(value);
            //costText.gameObject.SetActive(value);
        }

        #region 포인터이벤트

        public Timer_Skill timer;

        Vector3 resetPos = new Vector3();
        Sprite sprite;

        private bool isPush;
        public bool IsPush{get{return isPush;} 
            set {
                isPush = value;
                if(isPush) ColController.Instance.BeginDrag(transform);
        }}

        public void OnPointerDown(PointerEventData data)
        {
            if(!isLevel) return;
            isPush = false;
            timer.SetData(this, 1f);
            timer.gameObject.SetActive(true);
            
            resetPos = transform.position;
            
            sprite = skillIconImg.sprite;
        }

        public void OnPointerUp(PointerEventData data)
        {
            if(isPush)
            {
                ColController.Instance.EndDrag(transform);
                SetSkillMod();
            }
            
        }
        
        public void OnPointerExit(PointerEventData data)
        {
            timer.TimerEnd();
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
        //아이콘 <-> 스킬모양
        private void SetIconMod()
        {
            skillIconImg.sprite = ColController.Instance.testSkillspr;
            SamMod(true);
        }

        private void SetSkillMod()
        {
            transform.position = resetPos;
            skillIconImg.sprite = sprite;
            SamMod(false);
        }

        void SamMod(bool isIcon)
        {
            transform.GetChild(1).gameObject.SetActive(!isIcon);
            eleCost.SetActive(!isIcon);
            nameText.enabled = !isIcon;
            costText.enabled = !isIcon;
        }
        #endregion
}