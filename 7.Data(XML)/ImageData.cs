using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageData : MonoBehaviour
{
    private static ImageData _instance = null;

    public static ImageData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(ImageData)) as ImageData;

                if(_instance == null)
                {
                    Debug.LogError("There's no active ImageData object");
                }
            }
            return _instance;
        }
    }
    //직업
    public Sprite[] job = new Sprite[8];
    //NPC
    public Sprite[] npc = new Sprite[8];  
    //스킬
    public Sprite targetSolo;
    public Sprite targetMulti;
    
    public Sprite blood;
    public Sprite stun;
    public Sprite poison;
    public Sprite ele;

    public Sprite GetSprite(string type)
    {
        switch(type)
        {
            case "타겟솔로" : return targetSolo;
            case "타겟멀티" : return targetMulti;
            case "기절" : return stun;
            case "출혈" : return blood;
            case "독" : return poison;
            case "속성" : return ele;
            default :
                CodeBox.PrintError("ImageData - GetSprite () 유형 없음");
                return null;
        }
    }
    

    public Sprite skillIcon;
    public Sprite GetSprite(SkillType type)
    {
        switch(type)
        {
            case SkillType.NONE : return skillIcon;
            case SkillType.COMMON : return skillIcon;
            case SkillType.DAM : return skillIcon;
            case SkillType.SHD : return skillIcon;
            case SkillType.BUF : return skillIcon;
            case SkillType.DBUF : return skillIcon;
            case SkillType.SUPPOSDAM : return skillIcon;
            default :
                CodeBox.PrintError("ImageData - GetSprite_icon () 유형 없음");
                return null;
        }
    }
    
    public void SetData()
    {
        npc = Resources.LoadAll<Sprite>("5.NPC/npc");
    }
}
