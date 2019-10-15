using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightSkinPrefab : MonoBehaviour
{
    public GameObject frontHair;
    public GameObject backHair;
    public GameObject topBrow;
    public GameObject underBrow;
    public GameObject back;
    public GameObject pupil;

    public Skin skin;

    public void SetData(Skin _skin)
    {
        skin = _skin;

        frontHair.GetComponent<SpriteRenderer>().sprite = skin.frontHair;
        backHair.GetComponent<SpriteRenderer>().sprite = skin.backHair;

        SetMask(topBrow, skin.topBrow);
        underBrow.GetComponent<SpriteRenderer>().sprite = skin.underBrow;
        SetMask( back, skin.back);

        pupil.GetComponent<SpriteRenderer>().sprite = skin.pupil;
    }
    

    
    void SetMask(GameObject obj, Sprite sprite)
    {
        obj.GetComponent<SpriteRenderer>().sprite = sprite;
        obj.GetComponent<SpriteMask>().sprite = sprite;
    }
}

