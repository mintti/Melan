using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkillPrefab : MonoBehaviour
{
    public Text nameText;
    public Text explanText;
    public Image typeImg;

    public Transform infoTr;
    public GameObject iconObj;

    public void SetData(Skill sk)
    {
        nameText.text = sk.name;
        explanText.text = sk.explan;

        CodeBox.ClearList(infoTr);
        
        /*
        GameObject targetObj = AddChildInParent(infoTr, iconObj);
        Image targetImage = targetObj.GetComponent<Image>();
        switch(sk.target)
        {
            case Target.S :
                targetImage.s
                
        } */

        //소모 행동력
        int c = sk.cost;
        GameObject obj = CodeBox.AddChildInParent(infoTr, iconObj);
        Image image = obj.GetComponent<Image>();//아이콘을 투명하게
        var tempColor = image.color;
        tempColor.a = 0f;
        image.color = tempColor;
        Text text = obj.transform.GetComponentInChildren<Text>();
        text.text = string.Format("{0}", c);

        //필요한 속성
        if(sk.needP > 0) SetIconObj(CodeBox.AddChildInParent(infoTr, iconObj), "속성", sk.needP);
        

    }

    private void SetIconObj(GameObject obj, string type, int n)
    {
        Image img = obj.GetComponent<Image>();
        img.sprite = ImageData.Instance.GetSprite(type);

        if(n >= 2)
        {
            Text text = obj.transform.GetComponentInChildren<Text>();
            text.text = string.Format("{0}", n);
        }

    }

    
}
