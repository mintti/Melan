using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Skill : MonoBehaviour, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{   
    private KnightState ks;
    private Form form;
    public SkillInfo skillInfo;
    
    public Image skillIconImg;
    public Text nameText;
    public GameObject lockObj;//조건 부적합일때 활성

        
    private bool only_View;
    
    public void SetData(Form _form, KnightState _ks, SkillInfo _skillInfo)
    {
        form = _form;
        //기본정보
        ks = _ks;

        //스킬정보
        skillInfo = _skillInfo;
        skillIconImg.sprite = skillInfo.img;

        nameText.text = skillInfo.name;
    }


    //Lock의 여부
    public void SetLock(int knightLevel)
    {
        Lock(SkillData.Instance.Check(skillInfo.job, ks.k));  
    }
    public void SetLock(bool value)
    {
        Lock(value);
    }
        

    //레벨딸려 비활 || 적의 공격으로 비활
    private void Lock(bool value)
    {
        lockObj.SetActive(value);
        nameText.gameObject.SetActive(!value);
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
            form.SetInfoText(skillInfo.explan);

            isPush = false;
            timer.SetData(this, 0.5f);
            timer.gameObject.SetActive(true);
            
            resetPos = transform.position;
            
            sprite = skillIconImg.sprite;
        }

        public void OnPointerUp(PointerEventData data)
        {
            timer.TimerEnd();
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
            nameText.enabled = !isIcon;
        }
        #endregion
}