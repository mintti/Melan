using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class KnightSkinPrefab : MonoBehaviour
{
    //변경되는...
    public GameObject frontHair;
    public GameObject backHair;
    public GameObject topBrow;
    public GameObject underBrow;
    public GameObject back;
    public GameObject pupil;
    public Skin skin;

    //픽스된..
    public GameObject closeEye;

    private KnightMotion motion;
    private void Awake() {
        motion = GetComponent<KnightMotion>();
    }
    //캐릭터 바디 세팅
    public void SetData(Knight knight)
    {
        skin = knight.skin;

        frontHair.GetComponent<SpriteRenderer>().sprite = skin.frontHair;
        backHair.GetComponent<SpriteRenderer>().sprite = skin.backHair;

        SetMask(topBrow, skin.topBrow);
        underBrow.GetComponent<SpriteRenderer>().sprite = skin.underBrow;
        SetMask( back, skin.back);

        pupil.GetComponent<SpriteRenderer>().sprite = skin.pupil;
        
        SetClothes(knight.job);
        motion.StartAction();
    }
    
    void SetMask(GameObject obj, Sprite sprite)
    {
        obj.GetComponent<SpriteRenderer>().sprite = sprite;
        obj.GetComponent<SpriteMask>().sprite = sprite;
    }

    #region 의상
    public GameObject front_Hat;
    public GameObject back_Hat;

    public GameObject body_Clothes;
    public GameObject arm_Left1_Clothes;
    public GameObject arm_Left2_Clothes;
    public GameObject arm_Right1_Clothes;
    public GameObject arm_Right2_Clothes;
    
    public GameObject leg_Clothes;
    private Sprite[] clothes;
    public void SetClothes(int job)
    {
        string path = "1.Skin/10.Clothes/" +  TextData.Instance.job_Lan_Common[job];
        clothes = Resources.LoadAll<Sprite>(path);
        
        SetClothes(body_Clothes, "body");
        SetClothes(arm_Left1_Clothes, "arm1");
        SetClothes(arm_Right1_Clothes, "arm1");
        SetClothes(arm_Left2_Clothes, "arm2");
        SetClothes(arm_Right2_Clothes, "arm2");
        SetClothes(leg_Clothes, "leg");
    }

    private void SetClothes(GameObject obj, string name)
    {
        obj.GetComponent<SpriteRenderer>().sprite  = clothes.Single( s => s.name == name);
    }
    #endregion
    #region 모션
    public Transform faceDecoTr;
    public void SetMotion(string name, bool value)
    {
        int num = 0;
        switch(name)
        {
            case "blood":
                num = 0;
                break;
            case "poison":
                num = 1;
                break;
            default :
                break;
        }

        faceDecoTr.GetChild(num).gameObject.SetActive(value);
    }
    #endregion
}

