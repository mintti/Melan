using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public void SetData(Skin _skin)
    {
        skin = _skin;

        frontHair.GetComponent<SpriteRenderer>().sprite = skin.frontHair;
        backHair.GetComponent<SpriteRenderer>().sprite = skin.backHair;

        SetMask(topBrow, skin.topBrow);
        underBrow.GetComponent<SpriteRenderer>().sprite = skin.underBrow;
        SetMask( back, skin.back);

        pupil.GetComponent<SpriteRenderer>().sprite = skin.pupil;
        
        motion.StartAction();
    }
    
    void SetMask(GameObject obj, Sprite sprite)
    {
        obj.GetComponent<SpriteRenderer>().sprite = sprite;
        obj.GetComponent<SpriteMask>().sprite = sprite;
    }

    #region 모션

    #endregion
}

