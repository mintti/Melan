using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Skill : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
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
    /*
    public void SetLock(int knightLevel)
    {
        Lock(SkillData.Instance.Check(skillInfo.job, ks.k));  
    }
    */
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

    Vector3 resetPos = new Vector3();
    Sprite sprite;

    public void OnPointerDown(PointerEventData data)
    {
        form.SetInfoText(skillInfo.explan);
        sprite = skillIconImg.sprite;

        ColController.Instance.BeginDrag(transform);
        SetIconMod();
        ColController.Instance.SetData(this);

        resetPos = transform.position;
    }

    public void OnPointerUp(PointerEventData data)
    {
        ColController.Instance.EndDrag(transform);
        SetSkillMod();
    }
        
       
    //드래그 이벤트
    public void OnDrag(PointerEventData eventData)
    {
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